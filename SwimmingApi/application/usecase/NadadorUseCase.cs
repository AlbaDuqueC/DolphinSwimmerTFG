using SwimmingApi.Application.Dtos.Nadador;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Servicios;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad Nadador.
/// Aquí se define la lógica de negocio paso a paso con rollback en caso de error.
/// </summary>
public class NadadorUseCase : INadadorUseCase
{
    private readonly INadadorRepository _repository;
    private readonly INadadorEquipoRepository _nadadorEquipoRepository;
    private readonly EncryptionService _encryption;
    private readonly CacheService _cache;
    private readonly NadadorInfraValidation _validation;

    public NadadorUseCase(
        INadadorRepository repository,
        INadadorEquipoRepository nadadorEquipoRepository,
        EncryptionService encryption,
        CacheService cache,
        NadadorInfraValidation validation)
    {
        _repository = repository;
        _nadadorEquipoRepository = nadadorEquipoRepository;
        _encryption = encryption;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>Obtiene un nadador por su ID. Primero busca en caché.</summary>
    public async Task<NadadorResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCaché = _cache.GenerarClave("nadador", id);
        var resultado = _cache.Obtener<NadadorResponseDto>(claveCaché);

        if (resultado == null)
        {
            var nadador = await _repository.ObtenerPorIdAsync(id);
            resultado = nadador != null ? MapearAResponse(nadador) : null;

            if (resultado != null)
                _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>Obtiene la lista completa de nadadores activos.</summary>
    public async Task<IEnumerable<NadadorResponseDto>> ObtenerTodosAsync()
    {
        var claveCaché = _cache.GenerarClaveLista("nadador");
        var resultado = _cache.Obtener<IEnumerable<NadadorResponseDto>>(claveCaché);

        if (resultado == null)
        {
            var nadadores = await _repository.ObtenerTodosAsync();
            resultado = nadadores.Select(MapearAResponse);
            _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Crea un nuevo nadador.
    /// Valida el email, encripta la contraseña y limpia la caché de la lista.
    /// </summary>
    public async Task<NadadorResponseDto> CrearAsync(NadadorRequestDto dto)
    {
        var emailDisponible = await _validation.EmailDisponibleAsync(dto.Email);
        if (!emailDisponible)
            throw new InvalidOperationException("El email ya está registrado.");

        var nadador = new Nadador
        {
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            PasswordHash = _encryption.HashPassword(dto.Password),
            IdEquipo = dto.IdEquipo,
        };

        Nadador nadadorCreado;
        try
        {
            nadadorCreado = await _repository.CrearAsync(nadador);
        }
        catch
        {
            // Rollback: si falla la creación, no hay nada que revertir manualmente
            // EF Core no persiste nada si SaveChanges falla
            throw;
        }

        _cache.Eliminar(_cache.GenerarClaveLista("nadador"));
        var resultado = MapearAResponse(nadadorCreado);
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de un nadador existente.
    /// </summary>
    public async Task<NadadorResponseDto> ActualizarAsync(int id, NadadorRequestDto dto)
    {
        var nadador = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Nadador con ID {id} no encontrado.");

        nadador.Nombre = dto.Nombre;
        nadador.Apellidos = dto.Apellidos;
        nadador.Email = dto.Email;
        nadador.IdEquipo = dto.IdEquipo;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            nadador.PasswordHash = _encryption.HashPassword(dto.Password);

        var nadadorActualizado = await _repository.ActualizarAsync(nadador);

        _cache.Eliminar(_cache.GenerarClave("nadador", id));
        _cache.Eliminar(_cache.GenerarClaveLista("nadador"));

        var resultado = MapearAResponse(nadadorActualizado);
        return resultado;
    }

    /// <summary>Elimina lógicamente un nadador por su ID.</summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.NadadorExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"Nadador con ID {id} no encontrado.");

        var eliminado = await _repository.EliminarLogicoAsync(id);

        if (eliminado)
        {
            _cache.Eliminar(_cache.GenerarClave("nadador", id));
            _cache.Eliminar(_cache.GenerarClaveLista("nadador"));
        }

        var resultado = eliminado;
        return resultado;
    }

    /// <summary>Obtiene un nadador por su email.</summary>
    public async Task<NadadorResponseDto?> ObtenerPorEmailAsync(string email)
    {
        var nadador = await _repository.ObtenerPorEmailAsync(email);
        var resultado = nadador != null ? MapearAResponse(nadador) : null;
        return resultado;
    }

    /// <summary>
    /// Vincula un nadador (usuario) con un NadadorEquipo (ficha del equipo) usando el código
    /// que le ha dado el entrenador. Falla si el código no existe o si ya está siendo usado.
    /// </summary>
    public async Task<NadadorResponseDto> VincularConCodigoAsync(int idNadador, int codigo)
    {
        // 1) Buscar al nadador (usuario)
        var nadador = await _repository.ObtenerPorIdAsync(idNadador)
            ?? throw new KeyNotFoundException($"Nadador con ID {idNadador} no encontrado.");

        // 2) Comprobar que no esté ya vinculado
        if (nadador.IdNadadorEquipo != null)
            throw new InvalidOperationException("Ya estás vinculado a un equipo.");

        // 3) Buscar el NadadorEquipo por su código
        var nadadorEquipo = await _nadadorEquipoRepository.ObtenerPorCodigoAsync(codigo)
            ?? throw new KeyNotFoundException("No existe ningún nadador con ese código.");

        // 4) Comprobar que ese código no esté ya en uso por otro usuario
        var todos = await _repository.ObtenerTodosAsync();
        var ocupado = todos.Any(n => n.IdNadadorEquipo == nadadorEquipo.Id);
        if (ocupado)
            throw new InvalidOperationException("Ese código ya está siendo usado por otro nadador.");

        // 5) Vincular
        nadador.IdNadadorEquipo = nadadorEquipo.Id;
        nadador.IdEquipo = nadadorEquipo.IdEquipo;
        var actualizado = await _repository.ActualizarAsync(nadador);

        // 6) Limpiar caché
        _cache.Eliminar(_cache.GenerarClave("nadador", nadador.Id));

        return MapearAResponse(actualizado);
    }

    // Mapea la entidad Nadador al DTO de respuesta
    private NadadorResponseDto MapearAResponse(Nadador nadador)
    {
        var resultado = new NadadorResponseDto
        {
            Id = nadador.Id,
            IdNadador = nadador.Id,
            Nombre = nadador.Nombre,
            Apellidos = nadador.Apellidos,
            Email = nadador.Email,
            IdEquipo = nadador.IdEquipo,
            IdNadadorEquipo = nadador.IdNadadorEquipo,
            CreatedAt = nadador.CreatedAt,
            UpdateAt = nadador.UpdateAt
        };
        return resultado;
    }
}
