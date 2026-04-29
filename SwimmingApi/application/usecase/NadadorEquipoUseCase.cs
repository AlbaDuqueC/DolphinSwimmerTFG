using SwimmingApi.Application.Dtos.NadadorEquipo;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para NadadorEquipo.
/// Gestiona el registro de nadadores dentro de un equipo y genera su código único.
/// </summary>
public class NadadorEquipoUseCase : INadadorEquipoUseCase
{
    private readonly INadadorEquipoRepository _repository;
    private readonly CacheService _cache;
    private readonly NadadorEquipoInfraValidation _validation;

    public NadadorEquipoUseCase(
        INadadorEquipoRepository repository,
        CacheService cache,
        NadadorEquipoInfraValidation validation)
    {
        _repository = repository;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>Obtiene un NadadorEquipo por su ID.</summary>
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

    /// <summary>Obtiene un NadadorEquipo por su código de conexión.</summary>
    public async Task<NadadorEquipoResponseDto?> ObtenerPorCodigoAsync(int codigo)
    {
        var nadadorEquipo = await _repository.ObtenerPorCodigoAsync(codigo);
        var resultado = nadadorEquipo != null ? MapearAResponse(nadadorEquipo) : null;
        return resultado;
    }

    /// <summary>Obtiene todos los NadadoresEquipo de un equipo concreto.</summary>
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
    /// Crea un nuevo NadadorEquipo con un código único generado automáticamente.
    /// </summary>
    public async Task<NadadorEquipoResponseDto> CrearAsync(NadadorEquipoRequestDto dto)
    {
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

    /// <summary>Actualiza los datos de un NadadorEquipo.</summary>
    public async Task<NadadorEquipoResponseDto> ActualizarAsync(int id, NadadorEquipoRequestDto dto)
    {
        var nadadorEquipo = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"NadadorEquipo con ID {id} no encontrado.");

        nadadorEquipo.Nombre = dto.Nombre;
        nadadorEquipo.Apellidos = dto.Apellidos;

        var actualizado = await _repository.ActualizarAsync(nadadorEquipo);

        _cache.Eliminar(_cache.GenerarClave("nadadorequipo", id));
        _cache.Eliminar($"nadadorequipo:equipo:{nadadorEquipo.IdEquipo}");

        var resultado = MapearAResponse(actualizado);
        return resultado;
    }

    /// <summary>Elimina lógicamente un NadadorEquipo.</summary>
    public async Task<bool> EliminarAsync(int id)
    {
        var existe = await _validation.ExisteAsync(id);
        if (!existe)
            throw new KeyNotFoundException($"NadadorEquipo con ID {id} no encontrado.");

        var eliminado = await _repository.EliminarLogicoAsync(id);

        if (eliminado)
            _cache.Eliminar(_cache.GenerarClave("nadadorequipo", id));

        var resultado = eliminado;
        return resultado;
    }

    // Genera un código numérico único de 6 dígitos
    private async Task<int> GenerarCodigoUnicoAsync()
    {
        var random = new Random();
        int codigo;
        bool disponible;

        do
        {
            codigo = random.Next(100000, 999999);
            disponible = await _validation.CodigoDisponibleAsync(codigo);
        } while (!disponible);

        return codigo;
    }

    // Mapea NadadorEquipo al DTO de respuesta
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
