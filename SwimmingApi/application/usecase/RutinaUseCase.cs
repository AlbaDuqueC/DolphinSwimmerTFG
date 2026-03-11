using SwimmingApi.Application.Dtos.Rutina;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad Rutina.
/// </summary>
public class RutinaUseCase : IRutinaUseCase
{
    private readonly IRutinaRepository _repository;
    private readonly CacheService _cache;
    private readonly RutinaInfraValidation _validation;

    public RutinaUseCase(
        IRutinaRepository repository,
        CacheService cache,
        RutinaInfraValidation validation)
    {
        _repository = repository;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>Obtiene una rutina por su ID.</summary>
    public async Task<RutinaResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCaché = _cache.GenerarClave("rutina", id);
        var resultado = _cache.Obtener<RutinaResponseDto>(claveCaché);

        if (resultado == null)
        {
            var rutina = await _repository.ObtenerPorIdAsync(id);
            resultado = rutina != null ? MapearAResponse(rutina) : null;

            if (resultado != null)
                _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>Obtiene todas las rutinas de un usuario concreto.</summary>
    public async Task<IEnumerable<RutinaResponseDto>> ObtenerPorUsuarioAsync(int idUsuario)
    {
        var claveCaché = $"rutina:usuario:{idUsuario}";
        var resultado = _cache.Obtener<IEnumerable<RutinaResponseDto>>(claveCaché);

        if (resultado == null)
        {
            var rutinas = await _repository.ObtenerPorUsuarioAsync(idUsuario);
            resultado = rutinas.Select(MapearAResponse);
            _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>Crea una nueva rutina para un usuario.</summary>
    public async Task<RutinaResponseDto> CrearAsync(RutinaRequestDto dto)
    {
        var rutina = new Rutina
        {
            Contenido = dto.Contenido,
            Fecha = dto.Fecha,
            Mostrar = dto.Mostrar,
            IdUsuario = dto.IdUsuario
        };

        Rutina creada;
        try
        {
            creada = await _repository.CrearAsync(rutina);
        }
        catch
        {
            throw;
        }

        _cache.Eliminar($"rutina:usuario:{dto.IdUsuario}");
        var resultado = MapearAResponse(creada);
        return resultado;
    }

    /// <summary>Actualiza una rutina existente.</summary>
    public async Task<RutinaResponseDto> ActualizarAsync(int id, RutinaRequestDto dto)
    {
        var rutina = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Rutina con ID {id} no encontrada.");

        rutina.Contenido = dto.Contenido;
        rutina.Fecha = dto.Fecha;
        rutina.Mostrar = dto.Mostrar;

        var actualizada = await _repository.ActualizarAsync(rutina);

        _cache.Eliminar(_cache.GenerarClave("rutina", id));
        _cache.Eliminar($"rutina:usuario:{rutina.IdUsuario}");

        var resultado = MapearAResponse(actualizada);
        return resultado;
    }

    /// <summary>Elimina lógicamente una rutina.</summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.RutinaExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"Rutina con ID {id} no encontrada.");

        var eliminada = await _repository.EliminarLogicoAsync(id);

        if (eliminada)
            _cache.Eliminar(_cache.GenerarClave("rutina", id));

        var resultado = eliminada;
        return resultado;
    }

    // Mapea Rutina al DTO de respuesta
    private RutinaResponseDto MapearAResponse(Rutina rutina)
    {
        var resultado = new RutinaResponseDto
        {
            Id = rutina.Id,
            IdRutina = rutina.IdRutina,
            Contenido = rutina.Contenido,
            Fecha = rutina.Fecha,
            Mostrar = rutina.Mostrar,
            IdUsuario = rutina.IdUsuario,
            CreatedAt = rutina.CreatedAt,
            UpdateAt = rutina.UpdateAt
        };
        return resultado;
    }
}
