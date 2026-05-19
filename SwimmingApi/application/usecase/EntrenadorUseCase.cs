using SwimmingApi.Application.Dtos.Entrenador;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Servicios;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad Entrenador.
/// Aplica el patrón cache-aside: primero consulta la caché y solo recurre al
/// repositorio si no hay datos guardados, lo que reduce las consultas a la base de datos.
/// </summary>
public class EntrenadorUseCase : IEntrenadorUseCase
{
    // Acceso a la base de datos a través del repositorio.
    private readonly IEntrenadorRepository _repository;

    // Servicio que encripta las contraseñas con BCrypt antes de guardarlas.
    private readonly EncryptionService _encryption;

    // Servicio de caché en memoria para acelerar lecturas frecuentes.
    private readonly CacheService _cache;

    // Validaciones de infraestructura (ej: comprobar que el email no exista).
    private readonly EntrenadorInfraValidation _validation;

    /// <summary>
    /// Constructor con inyección de dependencias del repositorio,
    /// servicio de encriptación, caché y validador.
    /// </summary>
    public EntrenadorUseCase(
        IEntrenadorRepository repository,
        EncryptionService encryption,
        CacheService cache,
        EntrenadorInfraValidation validation)
    {
        _repository = repository;
        _encryption = encryption;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>
    /// Obtiene un entrenador por su ID. Primero comprueba la caché para evitar
    /// consultar la base de datos si el dato ya está guardado en memoria.
    /// </summary>
    public async Task<EntrenadorResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCaché = _cache.GenerarClave("entrenador", id);
        var resultado = _cache.Obtener<EntrenadorResponseDto>(claveCaché);

        // Si no está en caché, se consulta la BD y se guarda el resultado en caché.
        if (resultado == null)
        {
            var entrenador = await _repository.ObtenerPorIdAsync(id);
            resultado = entrenador != null ? MapearAResponse(entrenador) : null;

            if (resultado != null)
                _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Obtiene un entrenador por su email.
    /// Utilizado principalmente al iniciar sesión para identificar al usuario.
    /// </summary>
    public async Task<EntrenadorResponseDto?> ObtenerPorEmailAsync(string email)
    {
        var entrenador = await _repository.ObtenerPorEmailAsync(email);
        var resultado = entrenador != null ? MapearAResponse(entrenador) : null;
        return resultado;
    }

    /// <summary>
    /// Obtiene todos los entrenadores activos.
    /// La lista se cachea para acelerar lecturas posteriores.
    /// </summary>
    public async Task<IEnumerable<EntrenadorResponseDto>> ObtenerTodosAsync()
    {
        var claveCaché = _cache.GenerarClaveLista("entrenador");
        var resultado = _cache.Obtener<IEnumerable<EntrenadorResponseDto>>(claveCaché);

        if (resultado == null)
        {
            var entrenadores = await _repository.ObtenerTodosAsync();
            resultado = entrenadores.Select(MapearAResponse);
            _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Crea un nuevo entrenador validando que el email no esté ya registrado
    /// y encriptando la contraseña antes de guardarla en la base de datos.
    /// </summary>
    public async Task<EntrenadorResponseDto> CrearAsync(EntrenadorRequestDto dto)
    {
        // Se valida que el email esté disponible antes de crear el registro.
        var emailDisponible = await _validation.EmailDisponibleAsync(dto.Email);
        if (!emailDisponible)
            throw new InvalidOperationException("El email ya está registrado.");

        // Se construye la entidad y se encripta la contraseña.
        var entrenador = new Entrenador
        {
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            PasswordHash = _encryption.HashPassword(dto.Password),
            IdEquipoGestionado = dto.IdEquipoGestionado
        };

        Entrenador creado;
        try
        {
            creado = await _repository.CrearAsync(entrenador);
        }
        catch
        {
            throw;
        }

        // Se invalida la caché de la lista para que la próxima lectura refleje el nuevo registro.
        _cache.Eliminar(_cache.GenerarClaveLista("entrenador"));
        var resultado = MapearAResponse(creado);
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de un entrenador existente.
    /// Si la contraseña viene vacía, se conserva la actual sin tocarla.
    /// </summary>
    public async Task<EntrenadorResponseDto> ActualizarAsync(int id, EntrenadorRequestDto dto)
    {
        var entrenador = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Entrenador con ID {id} no encontrado.");

        entrenador.Nombre = dto.Nombre;
        entrenador.Apellidos = dto.Apellidos;
        entrenador.Email = dto.Email;
        entrenador.IdEquipoGestionado = dto.IdEquipoGestionado;

        // La contraseña solo se actualiza si llega informada (al editar perfil suele venir vacía).
        if (!string.IsNullOrWhiteSpace(dto.Password))
            entrenador.PasswordHash = _encryption.HashPassword(dto.Password);

        var actualizado = await _repository.ActualizarAsync(entrenador);

        // Se invalidan las cachés afectadas por el cambio.
        _cache.Eliminar(_cache.GenerarClave("entrenador", id));
        _cache.Eliminar(_cache.GenerarClaveLista("entrenador"));

        var resultado = MapearAResponse(actualizado);
        return resultado;
    }

    /// <summary>
    /// Elimina lógicamente un entrenador por su ID
    /// (el registro permanece en la base de datos pero se marca como inactivo).
    /// </summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.EntrenadorExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"Entrenador con ID {id} no encontrado.");

        var eliminado = await _repository.EliminarLogicoAsync(id);

        // Se limpia la caché del entrenador eliminado y la de la lista global.
        if (eliminado)
        {
            _cache.Eliminar(_cache.GenerarClave("entrenador", id));
            _cache.Eliminar(_cache.GenerarClaveLista("entrenador"));
        }

        var resultado = eliminado;
        return resultado;
    }

    /// <summary>
    /// Convierte una entidad Entrenador del dominio en su DTO de respuesta para la API.
    /// </summary>
    private EntrenadorResponseDto MapearAResponse(Entrenador entrenador)
    {
        var resultado = new EntrenadorResponseDto
        {
            Id = entrenador.Id,
            IdEntrenador = entrenador.Id,
            Nombre = entrenador.Nombre,
            Apellidos = entrenador.Apellidos,
            Email = entrenador.Email,
            IdEquipo = entrenador.IdEquipo,
            IdEquipoGestionado = entrenador.IdEquipoGestionado,
            CreatedAt = entrenador.CreatedAt,
            UpdateAt = entrenador.UpdateAt
        };
        return resultado;
    }
}