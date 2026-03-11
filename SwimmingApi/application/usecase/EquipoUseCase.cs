using SwimmingApi.Application.Dtos.Equipo;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad Equipo.
/// </summary>
public class EquipoUseCase : IEquipoUseCase
{
    private readonly IEquipoRepository _repository;
    private readonly CacheService _cache;
    private readonly EquipoInfraValidation _validation;

    public EquipoUseCase(
        IEquipoRepository repository,
        CacheService cache,
        EquipoInfraValidation validation)
    {
        _repository = repository;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>Obtiene un equipo por su ID.</summary>
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

    /// <summary>Obtiene todos los equipos activos.</summary>
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

    /// <summary>Crea un nuevo equipo.</summary>
    public async Task<EquipoResponseDto> CrearAsync(EquipoRequestDto dto)
    {
        var equipo = new Equipo
        {
            Nombre = dto.Nombre
        };

        Equipo creado;
        try
        {
            creado = await _repository.CrearAsync(equipo);
        }
        catch
        {
            throw;
        }

        _cache.Eliminar(_cache.GenerarClaveLista("equipo"));
        var resultado = MapearAResponse(creado);
        return resultado;
    }

    /// <summary>Actualiza el nombre de un equipo existente.</summary>
    public async Task<EquipoResponseDto> ActualizarAsync(int id, EquipoRequestDto dto)
    {
        var equipo = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Equipo con ID {id} no encontrado.");

        equipo.Nombre = dto.Nombre;

        var actualizado = await _repository.ActualizarAsync(equipo);

        _cache.Eliminar(_cache.GenerarClave("equipo", id));
        _cache.Eliminar(_cache.GenerarClaveLista("equipo"));

        var resultado = MapearAResponse(actualizado);
        return resultado;
    }

    /// <summary>Elimina lógicamente un equipo por su ID.</summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.EquipoExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"Equipo con ID {id} no encontrado.");

        var eliminado = await _repository.EliminarLogicoAsync(id);

        if (eliminado)
        {
            _cache.Eliminar(_cache.GenerarClave("equipo", id));
            _cache.Eliminar(_cache.GenerarClaveLista("equipo"));
        }

        var resultado = eliminado;
        return resultado;
    }

    // Mapea Equipo al DTO de respuesta
    private EquipoResponseDto MapearAResponse(Equipo equipo)
    {
        var resultado = new EquipoResponseDto
        {
            Id = equipo.Id,
            IdEquipo = equipo.IdEquipo,
            Nombre = equipo.Nombre,
            TotalNadadores = equipo.ListaNadadores.Count,
            CreatedAt = equipo.CreatedAt,
            UpdateAt = equipo.UpdateAt
        };
        return resultado;
    }
}
