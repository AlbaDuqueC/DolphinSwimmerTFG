using SwimmingApi.Application.Dtos.MarcaDeTiempo;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad MarcaDeTiempo.
/// Permite registrar una marca uno mismo o a través del entrenador.
/// </summary>
public class MarcaDeTiempoUseCase : IMarcaDeTiempoUseCase
{
    private readonly IMarcaRepository _repository;
    private readonly CacheService _cache;
    private readonly MarcaDeTiempoInfraValidation _validation;

    public MarcaDeTiempoUseCase(
        IMarcaRepository repository,
        CacheService cache,
        MarcaDeTiempoInfraValidation validation)
    {
        _repository = repository;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>Obtiene una marca de tiempo por su ID.</summary>
    public async Task<MarcaDeTiempoResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCaché = _cache.GenerarClave("marca", id);
        var resultado = _cache.Obtener<MarcaDeTiempoResponseDto>(claveCaché);

        if (resultado == null)
        {
            var marca = await _repository.ObtenerPorIdAsync(id);
            resultado = marca != null ? MapearAResponse(marca) : null;

            if (resultado != null)
                _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>Obtiene todas las marcas de tiempo de un NadadorEquipo.</summary>
    public async Task<IEnumerable<MarcaDeTiempoResponseDto>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo)
    {
        var claveCaché = $"marca:nadadorequipo:{idNadadorEquipo}";
        var resultado = _cache.Obtener<IEnumerable<MarcaDeTiempoResponseDto>>(claveCaché);

        if (resultado == null)
        {
            var marcas = await _repository.ObtenerPorNadadorEquipoAsync(idNadadorEquipo);
            resultado = marcas.Select(MapearAResponse);
            _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }
    /// <summary>Obtiene todas las marcas registradas por un nadador concreto.</summary>
    public async Task<IEnumerable<MarcaDeTiempoResponseDto>> ObtenerPorNadadorAsync(int idNadador)
    {
        var claveCaché = $"marca:nadador:{idNadador}";
        var resultado = _cache.Obtener<IEnumerable<MarcaDeTiempoResponseDto>>(claveCaché);

        if (resultado == null)
        {
            var marcas = await _repository.ObtenerPorNadadorAsync(idNadador);
            resultado = marcas.Select(MapearAResponse);
            _cache.Guardar(claveCaché, resultado);
        }

        return resultado;
    }

    /// <summary>
    /// Registra una nueva marca de tiempo para un NadadorEquipo.
    /// Puede ser registrada por el nadador o por el entrenador.
    /// </summary>
    public async Task<MarcaDeTiempoResponseDto> CrearAsync(MarcaDeTiempoRequestDto dto)
    {
        var marca = new MarcaDeTiempo
        {
            Tiempo = dto.Tiempo,
            Descripcion = dto.Descripcion,
            IdNadadorEquipo = dto.IdNadadorEquipo,
            IdNadador = dto.IdNadador
        };

        MarcaDeTiempo creada;
        try
        {
            creada = await _repository.CrearAsync(marca);
        }
        catch
        {
            throw;
        }

        _cache.Eliminar($"marca:nadadorequipo:{dto.IdNadadorEquipo}");
        if (dto.IdNadador.HasValue)
            _cache.Eliminar($"marca:nadador:{dto.IdNadador}");
        var resultado = MapearAResponse(creada);
        return resultado;
    }

    /// <summary>Actualiza una marca de tiempo existente.</summary>
    public async Task<MarcaDeTiempoResponseDto> ActualizarAsync(int id, MarcaDeTiempoRequestDto dto)
    {
        var marca = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"MarcaDeTiempo con ID {id} no encontrada.");

        marca.Tiempo = dto.Tiempo;
        marca.Descripcion = dto.Descripcion;

        var actualizada = await _repository.ActualizarAsync(marca);

        _cache.Eliminar(_cache.GenerarClave("marca", id));
        _cache.Eliminar($"marca:nadadorequipo:{marca.IdNadadorEquipo}");

        var resultado = MapearAResponse(actualizada);
        return resultado;
    }

    /// <summary>Elimina lógicamente una marca de tiempo.</summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.MarcaExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"MarcaDeTiempo con ID {id} no encontrada.");

        var eliminada = await _repository.EliminarLogicoAsync(id);

        if (eliminada)
            _cache.Eliminar(_cache.GenerarClave("marca", id));

        var resultado = eliminada;
        return resultado;
    }

    // Mapea MarcaDeTiempo al DTO de respuesta
    private MarcaDeTiempoResponseDto MapearAResponse(MarcaDeTiempo marca)
    {
        var resultado = new MarcaDeTiempoResponseDto
        {
            Id = marca.Id,
            IdMarca = marca.Id,
            Tiempo = marca.Tiempo,
            Descripcion = marca.Descripcion,
            IdNadadorEquipo = marca.IdNadadorEquipo,
            IdNadador = marca.IdNadador,
            CreatedAt = marca.CreatedAt,
            UpdateAt = marca.UpdateAt
        };
        return resultado;
    }
}
