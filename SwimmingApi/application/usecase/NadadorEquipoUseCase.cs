using SwimmingApi.Application.Dtos.NadadorEquipo;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para NadadorEquipo.
/// Gestiona el registro de nadadores dentro de un equipo y se encarga
/// de generar un código único de 6 dígitos que el nadador real usará para vincularse.
/// </summary>
public class NadadorEquipoUseCase : INadadorEquipoUseCase
{
    // Acceso a la base de datos a través del repositorio de NadadorEquipo.
    private readonly INadadorEquipoRepository _repository;

    // Repositorio de nadadores, necesario para desvincular usuarios al eliminar fichas.
    private readonly INadadorRepository _nadadorRepository;

    // Servicio de caché en memoria.
    private readonly CacheService _cache;

    // Validaciones de infraestructura para NadadorEquipo (ej: comprobar códigos únicos).
    private readonly NadadorEquipoInfraValidation _validation;

    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public NadadorEquipoUseCase(
        INadadorEquipoRepository repository,
        INadadorRepository nadadorRepository,
        CacheService cache,
        NadadorEquipoInfraValidation validation)
    {
        _repository = repository;
        _nadadorRepository = nadadorRepository;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>
    /// Obtiene un NadadorEquipo por su ID.
    /// Aplica el patrón cache-aside.
    /// </summary>
    public async Task<NadadorEquipoResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCaché = _cache.GenerarClave("nadadorequipo", id);
        var resultado = _cache.Obtener<NadadorEquipoResponseDto>(claveCaché);

        if (resultado == null)
        {
            var nadadorEquipo = await _repository.ObtenerPorIdAsync(id);
            resultado = nadadorEquipo != null ? MapearAResponse(nadadorEquipo) : null;

            if (resultado != null)
                _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Obtiene un NadadorEquipo a partir del código de conexión que introduce el nadador.
    /// </summary>
    public async Task<NadadorEquipoResponseDto?> ObtenerPorCodigoAsync(int codigo)
    {
        var nadadorEquipo = await _repository.ObtenerPorCodigoAsync(codigo);
        var resultado = nadadorEquipo != null ? MapearAResponse(nadadorEquipo) : null;
        return resultado;
    }

    /// <summary>
    /// Obtiene todos los NadadoresEquipo de un equipo concreto.
    /// La lista se cachea para acelerar lecturas posteriores.
    /// </summary>
    public async Task<IEnumerable<NadadorEquipoResponseDto>> ObtenerPorEquipoAsync(int idEquipo)
    {
        var claveCaché = $"nadadorequipo:equipo:{idEquipo}";
        var resultado = _cache.Obtener<IEnumerable<NadadorEquipoResponseDto>>(claveCaché);

        if (resultado == null)
        {
            var lista = await _repository.ObtenerPorEquipoAsync(idEquipo);
            resultado = lista.Select(MapearAResponse);
            _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Crea un nuevo NadadorEquipo asignándole un código único de 6 dígitos
    /// que el nadador real usará para vincular su cuenta al equipo.
    /// </summary>
    public async Task<NadadorEquipoResponseDto> CrearAsync(NadadorEquipoRequestDto dto)
    {
        // Se genera un código único antes de crear el registro.
        var codigo = await GenerarCodigoUnicoAsync();

        var nadadorEquipo = new NadadorEquipo
        {
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            IdEquipo = dto.IdEquipo,
            Codigo = codigo
        };

        NadadorEquipo creado;
        try
        {
            creado = await _repository.CrearAsync(nadadorEquipo);
        }
        catch
        {
            throw;
        }

        _cache.Eliminar($"nadadorequipo:equipo:{dto.IdEquipo}");
        var resultado = MapearAResponse(creado);
        return resultado;
    }

    /// <summary>
    /// Actualiza el nombre y los apellidos de un NadadorEquipo existente.
    /// </summary>
    public async Task<NadadorEquipoResponseDto> ActualizarAsync(int id, NadadorEquipoRequestDto dto)
    {
        var nadadorEquipo = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"NadadorEquipo con ID {id} no encontrado.");

        nadadorEquipo.Nombre = dto.Nombre;
        nadadorEquipo.Apellidos = dto.Apellidos;

        var actualizado = await _repository.ActualizarAsync(nadadorEquipo);

        // Se invalidan las cachés afectadas por el cambio.
        _cache.Eliminar(_cache.GenerarClave("nadadorequipo", id));
        _cache.Eliminar($"nadadorequipo:equipo:{nadadorEquipo.IdEquipo}");

        var resultado = MapearAResponse(actualizado);
        return resultado;
    }

    /// <summary>
    /// Elimina lógicamente un NadadorEquipo.
    /// Si había un Nadador (usuario) vinculado a esa ficha, lo desvincula del equipo
    /// para que vuelva al estado "sin equipo".
    /// </summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var nadadorEquipo = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"NadadorEquipo con ID {id} no encontrado.");

        var idEquipo = nadadorEquipo.IdEquipo;

        // Se busca si algún Nadador (usuario real) está vinculado a esta ficha.
        // Si lo encuentra, se le quita el equipo para que vuelva al estado "sin equipo".
        var todosLosNadadores = await _nadadorRepository.ObtenerTodosAsync();
        var usuarioVinculado = todosLosNadadores.FirstOrDefault(n => n.IdNadadorEquipo == id);
        if (usuarioVinculado != null)
        {
            usuarioVinculado.IdNadadorEquipo = null;
            usuarioVinculado.IdEquipo = null;
            await _nadadorRepository.ActualizarAsync(usuarioVinculado);
            _cache.Eliminar(_cache.GenerarClave("nadador", usuarioVinculado.Id));
        }

        var eliminado = await _repository.EliminarLogicoAsync(id);

        // Se invalidan las cachés afectadas tras la eliminación.
        if (eliminado)
        {
            _cache.Eliminar(_cache.GenerarClave("nadadorequipo", id));
            _cache.Eliminar($"nadadorequipo:equipo:{idEquipo}");
        }

        return eliminado;
    }

    /// <summary>
    /// Genera un código numérico único de 6 dígitos para identificar un NadadorEquipo.
    /// Repite el proceso hasta encontrar un código que no esté ya en uso.
    /// </summary>
    private async Task<int> GenerarCodigoUnicoAsync()
    {
        var random = new Random();
        int codigo;
        bool disponible;

        // Bucle: se generan códigos aleatorios hasta encontrar uno libre.
        do
        {
            codigo = random.Next(100000, 999999);
            disponible = await _validation.CodigoDisponibleAsync(codigo);
        } while (!disponible);

        return codigo;
    }

    /// <summary>
    /// Convierte una entidad NadadorEquipo del dominio en su DTO de respuesta para la API.
    /// </summary>
    private NadadorEquipoResponseDto MapearAResponse(NadadorEquipo ne)
    {
        var resultado = new NadadorEquipoResponseDto
        {
            Id = ne.Id,
            IdNadadorEquipo = ne.Id,
            Nombre = ne.Nombre,
            Apellidos = ne.Apellidos,
            Codigo = ne.Codigo,
            IdEquipo = ne.IdEquipo,
            CreatedAt = ne.CreatedAt,
            UpdateAt = ne.UpdateAt
        };
        return resultado;
    }
}