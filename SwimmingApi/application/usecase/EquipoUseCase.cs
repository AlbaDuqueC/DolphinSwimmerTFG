using SwimmingApi.Application.Dtos.Equipo;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad Equipo.
/// Gestiona el ciclo de vida de los equipos y su vinculación con entrenadores.
/// </summary>
public class EquipoUseCase : IEquipoUseCase
{
    // Acceso a la base de datos a través del repositorio de equipos.
    private readonly IEquipoRepository _repository;

    // Repositorio de entrenadores, usado para vincular un equipo a su creador.
    private readonly IEntrenadorRepository _entrenadorRepository;

    // Servicio de caché en memoria.
    private readonly CacheService _cache;

    // Validaciones de infraestructura para equipos.
    private readonly EquipoInfraValidation _validation;

    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public EquipoUseCase(
        IEquipoRepository repository,
        IEntrenadorRepository entrenadorRepository,
        CacheService cache,
        EquipoInfraValidation validation)
    {
        _repository = repository;
        _entrenadorRepository = entrenadorRepository;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>
    /// Obtiene un equipo por su ID.
    /// Aplica el patrón cache-aside: consulta primero la caché.
    /// </summary>
    public async Task<EquipoResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCaché = _cache.GenerarClave("equipo", id);
        var resultado = _cache.Obtener<EquipoResponseDto>(claveCaché);

        if (resultado == null)
        {
            var equipo = await _repository.ObtenerPorIdAsync(id);
            resultado = equipo != null ? MapearAResponse(equipo) : null;

            if (resultado != null)
                _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Obtiene todos los equipos activos.
    /// La lista se cachea para acelerar lecturas posteriores.
    /// </summary>
    public async Task<IEnumerable<EquipoResponseDto>> ObtenerTodosAsync()
    {
        var claveCaché = _cache.GenerarClaveLista("equipo");
        var resultado = _cache.Obtener<IEnumerable<EquipoResponseDto>>(claveCaché);

        if (resultado == null)
        {
            var equipos = await _repository.ObtenerTodosAsync();
            resultado = equipos.Select(MapearAResponse);
            _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Crea un nuevo equipo. Si en el DTO viene un IdEntrenador,
    /// vincula automáticamente el equipo a ese entrenador como su equipo gestionado.
    /// </summary>
    public async Task<EquipoResponseDto> CrearAsync(EquipoRequestDto dto)
    {
        // Se crea el equipo con el nombre indicado.
        var equipo = new Equipo
        {
            Nombre = dto.Nombre
        };

        var creado = await _repository.CrearAsync(equipo);

        // Si el creador es un entrenador, se vincula el equipo a su perfil
        // para que lo gestione y aparezca como su equipo activo.
        if (dto.IdEntrenador.HasValue)
        {
            var entrenador = await _entrenadorRepository.ObtenerPorIdAsync(dto.IdEntrenador.Value);
            if (entrenador != null)
            {
                entrenador.IdEquipoGestionado = creado.Id;
                entrenador.IdEquipo = creado.Id;
                await _entrenadorRepository.ActualizarAsync(entrenador);
                _cache.Eliminar(_cache.GenerarClave("entrenador", entrenador.Id));
            }
        }

        _cache.Eliminar(_cache.GenerarClaveLista("equipo"));
        var resultado = MapearAResponse(creado);
        return resultado;
    }

    /// <summary>
    /// Actualiza el nombre de un equipo existente.
    /// </summary>
    public async Task<EquipoResponseDto> ActualizarAsync(int id, EquipoRequestDto dto)
    {
        var equipo = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Equipo con ID {id} no encontrado.");

        equipo.Nombre = dto.Nombre;

        var actualizado = await _repository.ActualizarAsync(equipo);

        // Se invalidan las cachés afectadas por el cambio.
        _cache.Eliminar(_cache.GenerarClave("equipo", id));
        _cache.Eliminar(_cache.GenerarClaveLista("equipo"));

        var resultado = MapearAResponse(actualizado);
        return resultado;
    }

    /// <summary>
    /// Elimina lógicamente un equipo por su ID.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.EquipoExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"Equipo con ID {id} no encontrado.");

        var eliminado = await _repository.EliminarLogicoAsync(id);

        // Se limpia la caché del equipo eliminado y la de la lista global.
        if (eliminado)
        {
            _cache.Eliminar(_cache.GenerarClave("equipo", id));
            _cache.Eliminar(_cache.GenerarClaveLista("equipo"));
        }

        var resultado = eliminado;
        return resultado;
    }

    /// <summary>
    /// Convierte una entidad Equipo del dominio en su DTO de respuesta para la API.
    /// Calcula también el número total de nadadores que pertenecen al equipo.
    /// </summary>
    private EquipoResponseDto MapearAResponse(Equipo equipo)
    {
        var resultado = new EquipoResponseDto
        {
            Id = equipo.Id,
            IdEquipo = equipo.Id,
            Nombre = equipo.Nombre,
            TotalNadadores = equipo.ListaNadadores.Count,
            CreatedAt = equipo.CreatedAt,
            UpdateAt = equipo.UpdateAt
        };
        return resultado;
    }
}