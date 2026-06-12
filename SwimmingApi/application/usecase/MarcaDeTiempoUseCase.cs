using SwimmingApi.Application.Dtos.MarcaDeTiempo;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad MarcaDeTiempo.
/// Permite registrar una marca por uno mismo (el nadador) o asignarla a través del entrenador.
/// </summary>
public class MarcaDeTiempoUseCase : IMarcaDeTiempoUseCase
{
    // Acceso a la base de datos a través del repositorio de marcas.
    private readonly IMarcaRepository _repository;

    // Servicio de caché en memoria.
    private readonly CacheService _cache;

    // Validaciones de infraestructura para marcas de tiempo.
    private readonly MarcaDeTiempoInfraValidation _validation;

    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public MarcaDeTiempoUseCase(
        IMarcaRepository repository,
        CacheService cache,
        MarcaDeTiempoInfraValidation validation)
    {
        _repository = repository;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>
    /// Obtiene una marca de tiempo por su ID.
    /// Aplica el patrón cache-aside.
    /// </summary>
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

    /// <summary>
    /// Obtiene todas las marcas de tiempo asociadas a un NadadorEquipo concreto.
    /// Incluye tanto las que el nadador se ha registrado a sí mismo
    /// como las que le ha asignado el entrenador.
    /// </summary>
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

    /// <summary>
    /// Obtiene todas las marcas registradas directamente por un nadador concreto.
    /// </summary>
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
    /// Registra una nueva marca de tiempo asociada a un NadadorEquipo.
    /// Si en el DTO se incluye IdNadador, la registra el propio nadador.
    /// Si no, se entiende que la ha asignado el entrenador.
    /// </summary>
    public async Task<MarcaDeTiempoResponseDto> CrearAsync(MarcaDeTiempoRequestDto dto)
    {
        // Se construye la entidad a partir del DTO.
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

        // Se invalidan las cachés relacionadas para reflejar la nueva marca.
        _cache.Eliminar($"marca:nadadorequipo:{dto.IdNadadorEquipo}");
        if (dto.IdNadador.HasValue)
            _cache.Eliminar($"marca:nadador:{dto.IdNadador}");
        var resultado = MapearAResponse(creada);
        return resultado;
    }

    /// <summary>
    /// Actualiza el tiempo o la descripción de una marca existente.
    /// </summary>
    public async Task<MarcaDeTiempoResponseDto> ActualizarAsync(int id, MarcaDeTiempoRequestDto dto)
    {
        var marca = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"MarcaDeTiempo con ID {id} no encontrada.");

        marca.Tiempo = dto.Tiempo;
        marca.Descripcion = dto.Descripcion;

        var actualizada = await _repository.ActualizarAsync(marca);

        // Se invalidan las cachés afectadas por el cambio.
        _cache.Eliminar(_cache.GenerarClave("marca", id));
        _cache.Eliminar($"marca:nadadorequipo:{marca.IdNadadorEquipo}");
        if (marca.IdNadador.HasValue)
            _cache.Eliminar($"marca:nadador:{marca.IdNadador}");

        var resultado = MapearAResponse(actualizada);
        return resultado;
    }

    /// <summary>
    /// Elimina lógicamente una marca de tiempo.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.MarcaExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"MarcaDeTiempo con ID {id} no encontrada.");

        // Se obtiene la marca ANTES de eliminarla para saber qué cachés de lista invalidar
        // (después de eliminarla, ObtenerPorIdAsync podría no devolverla).
        var marca = await _repository.ObtenerPorIdAsync(id);

        var eliminada = await _repository.EliminarLogicoAsync(id);

        if (eliminada)
        {
            // Invalida la caché de la marca individual.
            _cache.Eliminar(_cache.GenerarClave("marca", id));

            // Invalida también las cachés de LISTAS donde aparecía esta marca;
            // sin esto, ObtenerPorNadadorEquipoAsync/ObtenerPorNadadorAsync seguían
            // devolviendo la marca ya eliminada hasta que la caché expirase sola.
            if (marca != null)
            {
                _cache.Eliminar($"marca:nadadorequipo:{marca.IdNadadorEquipo}");
                if (marca.IdNadador.HasValue)
                    _cache.Eliminar($"marca:nadador:{marca.IdNadador}");
            }
        }

        var resultado = eliminada;
        return resultado;
    }

    /// <summary>
    /// Convierte una entidad MarcaDeTiempo del dominio en su DTO de respuesta para la API.
    /// </summary>
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
