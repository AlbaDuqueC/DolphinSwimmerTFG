using SwimmingApi.Application.Dtos.Rutina;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Application.Interfaces.UseCase;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Infraestructura.Rider;
using SwimmingApi.Infraestructura.Validaciones;
using Microsoft.EntityFrameworkCore;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Application.UseCase;

/// <summary>
/// Casos de uso para la entidad Rutina.
/// Cuando un entrenador crea una rutina, esta se replica automáticamente
/// para todos los nadadores de su equipo.
/// </summary>
public class RutinaUseCase : IRutinaUseCase
{
    // Acceso a la base de datos a través del repositorio de rutinas.
    private readonly IRutinaRepository _repository;

    // Repositorio de entrenadores, para saber si el creador gestiona un equipo.
    private readonly IEntrenadorRepository _entrenadorRepository;

    // Contexto de Entity Framework, usado para consultar nadadores del equipo
    // cuando hay que replicar rutinas a todo el equipo.
    private readonly AppDbContext _context;

    // Servicio de caché en memoria.
    private readonly CacheService _cache;

    // Validaciones de infraestructura para rutinas.
    private readonly RutinaInfraValidation _validation;

    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public RutinaUseCase(
        IRutinaRepository repository,
        IEntrenadorRepository entrenadorRepository,
        AppDbContext context,
        CacheService cache,
        RutinaInfraValidation validation)
    {
        _repository = repository;
        _entrenadorRepository = entrenadorRepository;
        _context = context;
        _cache = cache;
        _validation = validation;
    }

    /// <summary>
    /// Obtiene una rutina por su ID.
    /// Aplica el patrón cache-aside.
    /// </summary>
    public async Task<RutinaResponseDto?> ObtenerPorIdAsync(int id)
    {
        var claveCache = _cache.GenerarClave("rutina", id);
        var resultado = _cache.Obtener<RutinaResponseDto>(claveCache);
        if (resultado == null)
        {
            var rutina = await _repository.ObtenerPorIdAsync(id);
            resultado = rutina != null ? MapearAResponse(rutina) : null;
            if (resultado != null)
                _cache.Guardar(claveCache, resultado);
        }
        return resultado;
    }

    /// <summary>
    /// Obtiene todas las rutinas asociadas a un usuario concreto.
    /// La lista se cachea para acelerar lecturas posteriores.
    /// </summary>
    public async Task<IEnumerable<RutinaResponseDto>> ObtenerPorUsuarioAsync(int idUsuario)
    {
        var claveCache = $"rutina:usuario:{idUsuario}";
        var resultado = _cache.Obtener<IEnumerable<RutinaResponseDto>>(claveCache);
        if (resultado == null)
        {
            var rutinas = await _repository.ObtenerPorUsuarioAsync(idUsuario);
            resultado = rutinas.Select(MapearAResponse);
            _cache.Guardar(claveCache, resultado);
        }
        return resultado;
    }

    /// <summary>
    /// Crea una nueva rutina. Si el usuario que la crea es un entrenador con equipo gestionado,
    /// la rutina se replica también para todos los nadadores del equipo,
    /// de forma que cada uno la vea en su propio listado.
    /// El campo Contenido se rellena automáticamente con el Titulo para mantener
    /// la compatibilidad con datos anteriores.
    /// </summary>
    public async Task<RutinaResponseDto> CrearAsync(RutinaRequestDto dto)
    {
        // Contenido se sincroniza con el Titulo para compatibilidad con datos existentes.
        var contenidoFinal = !string.IsNullOrWhiteSpace(dto.Titulo) ? dto.Titulo : dto.Contenido;

        // 1) Se crea la rutina para el propio creador (entrenador o nadador).
        var rutinaCreador = new Rutina
        {
            Titulo = dto.Titulo,
            Descripcion = dto.Descripcion,
            Contenido = contenidoFinal,
            Fecha = dto.Fecha,
            Mostrar = dto.Mostrar,
            IdUsuario = dto.IdUsuario
        };
        var creada = await _repository.CrearAsync(rutinaCreador);

        // 2) Si el creador es un entrenador con equipo, se replica la rutina
        //    para todos los nadadores del equipo (uno por usuario).
        var entrenador = await _entrenadorRepository.ObtenerPorIdAsync(dto.IdUsuario);
        if (entrenador != null && entrenador.IdEquipoGestionado.HasValue)
        {
            // Se buscan todos los nadadores (usuarios) cuyo IdEquipo coincida con el del entrenador.
            var idEquipo = entrenador.IdEquipoGestionado.Value;
            var nadadores = await _context.Nadadores
                .Where(n => n.IdEquipo == idEquipo)
                .ToListAsync();

            // Se crea una copia idéntica de la rutina para cada nadador del equipo.
            foreach (var nadador in nadadores)
            {
                var copia = new Rutina
                {
                    Titulo = dto.Titulo,
                    Descripcion = dto.Descripcion,
                    Contenido = contenidoFinal,
                    Fecha = dto.Fecha,
                    Mostrar = dto.Mostrar,
                    IdUsuario = nadador.Id
                };
                await _repository.CrearAsync(copia);
                _cache.Eliminar($"rutina:usuario:{nadador.Id}");
            }
        }

        _cache.Eliminar($"rutina:usuario:{dto.IdUsuario}");
        var resultado = MapearAResponse(creada);
        return resultado;
    }

    /// <summary>
    /// Actualiza el título, la descripción, la fecha o la visibilidad de una rutina existente.
    /// </summary>
    public async Task<RutinaResponseDto> ActualizarAsync(int id, RutinaRequestDto dto)
    {
        var rutina = await _repository.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Rutina con ID {id} no encontrada.");

        rutina.Titulo = dto.Titulo;
        rutina.Descripcion = dto.Descripcion;
        // Contenido se sincroniza con el Titulo para compatibilidad.
        rutina.Contenido = !string.IsNullOrWhiteSpace(dto.Titulo) ? dto.Titulo : dto.Contenido;
        rutina.Fecha = dto.Fecha;
        rutina.Mostrar = dto.Mostrar;

        var actualizada = await _repository.ActualizarAsync(rutina);

        // Se invalidan las cachés afectadas por el cambio.
        _cache.Eliminar(_cache.GenerarClave("rutina", id));
        _cache.Eliminar($"rutina:usuario:{rutina.IdUsuario}");

        var resultado = MapearAResponse(actualizada);
        return resultado;
    }

    /// <summary>
    /// Elimina lógicamente una rutina.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
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

    /// <summary>
    /// Convierte una entidad Rutina del dominio en su DTO de respuesta para la API.
    /// </summary>
    private RutinaResponseDto MapearAResponse(Rutina rutina)
    {
        var resultado = new RutinaResponseDto
        {
            Id = rutina.Id,
            IdRutina = rutina.Id,
            Titulo = rutina.Titulo,
            Descripcion = rutina.Descripcion,
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
