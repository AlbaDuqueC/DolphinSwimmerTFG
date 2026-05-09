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
/// </summary>
public class EntrenadorUseCase : IEntrenadorUseCase
{
    private readonly IEntrenadorRepository _repository;
    private readonly EncryptionService _encryption;
    private readonly CacheService _cache;
    private readonly EntrenadorInfraValidation _validation;

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

    /// <summary>Obtiene un entrenador por su ID. Primero comprueba la caché.</summary>
    public async Task<EntrenadorResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCaché = _cache.GenerarClave("entrenador", id);
        var resultado = _cache.Obtener<EntrenadorResponseDto>(claveCaché);

        if (resultado == null)
        {
            var entrenador = await _repository.ObtenerPorIdAsync(id);
            resultado = entrenador != null ? MapearAResponse(entrenador) : null;

            if (resultado != null)
                _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }



    /// <summary>Obtiene un entrenador por su email.</summary>
    public async Task<EntrenadorResponseDto?> ObtenerPorEmailAsync(string email)
    {
        var entrenador = await _repository.ObtenerPorEmailAsync(email);
        var resultado = entrenador != null ? MapearAResponse(entrenador) : null;
        return resultado;
    }

    /// <summary>Obtiene todos los entrenadores activos.</summary>
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

    /// <summary>Crea un nuevo entrenador validando el email y encriptando la contraseña.</summary>
    public async Task<EntrenadorResponseDto> CrearAsync(EntrenadorRequestDto dto)
    {
        var emailDisponible = await _validation.EmailDisponibleAsync(dto.Email);
        if (!emailDisponible)
            throw new InvalidOperationException("El email ya está registrado.");

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

        _cache.Eliminar(_cache.GenerarClaveLista("entrenador"));
        var resultado = MapearAResponse(creado);
        return resultado;
    }

    /// <summary>Actualiza los datos de un entrenador existente.</summary>
    public async Task<EntrenadorResponseDto> ActualizarAsync(int id, EntrenadorRequestDto dto)
    {
        var entrenador = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Entrenador con ID {id} no encontrado.");

        entrenador.Nombre = dto.Nombre;
        entrenador.Apellidos = dto.Apellidos;
        entrenador.Email = dto.Email;
        entrenador.IdEquipoGestionado = dto.IdEquipoGestionado;

        if (!string.IsNullOrWhiteSpace(dto.Password))
            entrenador.PasswordHash = _encryption.HashPassword(dto.Password);

        var actualizado = await _repository.ActualizarAsync(entrenador);

        _cache.Eliminar(_cache.GenerarClave("entrenador", id));
        _cache.Eliminar(_cache.GenerarClaveLista("entrenador"));

        var resultado = MapearAResponse(actualizado);
        return resultado;
    }

    /// <summary>Elimina lógicamente un entrenador por su ID.</summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.EntrenadorExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"Entrenador con ID {id} no encontrado.");

        var eliminado = await _repository.EliminarLogicoAsync(id);

        if (eliminado)
        {
            _cache.Eliminar(_cache.GenerarClave("entrenador", id));
            _cache.Eliminar(_cache.GenerarClaveLista("entrenador"));
        }

        var resultado = eliminado;
        return resultado;
    }

    // Mapea la entidad Entrenador al DTO de respuesta
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
