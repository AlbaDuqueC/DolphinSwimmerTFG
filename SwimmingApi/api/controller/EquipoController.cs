using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Equipo;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador REST para operaciones sobre Equipos.
/// Gestiona el ciclo de vida del equipo: creación, consulta, edición y eliminación.
/// Solo conoce la capa Application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EquipoController : ControllerBase
{
    // Caso de uso que contiene la lógica de negocio para equipos.
    private readonly IEquipoUseCase _useCase;

    /// <summary>
    /// Constructor con inyección de dependencias del caso de uso.
    /// </summary>
    public EquipoController(IEquipoUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene la lista de todos los equipos activos del sistema.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EquipoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerTodos()
    {
        IActionResult salida;
        try
        {
            var equipos = await _useCase.ObtenerTodosAsync();
            if (!equipos.Any())
            {
                salida = NoContent();
            }
            else
            {
                salida = Ok(ApiResponse<IEnumerable<EquipoResponseDto>>.Ok(equipos));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene un equipo por su ID.
    /// Devuelve 404 si no existe.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        IActionResult salida;
        try
        {
            var equipo = await _useCase.ObtenerPorIdAsync(id);
            if (equipo == null)
            {
                salida = NotFound(ApiResponse<EquipoResponseDto>.Error($"Equipo con ID {id} no encontrado."));
            }
            else
            {
                salida = Ok(ApiResponse<EquipoResponseDto>.Ok(equipo));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Crea un nuevo equipo.
    /// El equipo queda vinculado al entrenador que lo crea.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EquipoResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] EquipoRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var equipo = await _useCase.CrearAsync(dto);
            salida = CreatedAtAction(nameof(ObtenerPorId), new { id = equipo.Id },
                ApiResponse<EquipoResponseDto>.Ok(equipo, "Equipo creado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Actualiza el nombre de un equipo existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EquipoRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var equipo = await _useCase.ActualizarAsync(id, dto);
            salida = Ok(ApiResponse<EquipoResponseDto>.Ok(equipo, "Equipo actualizado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Elimina lógicamente un equipo.
    /// El registro permanece en la base de datos pero se marca como inactivo,
    /// preservando el histórico de nadadores y marcas asociadas.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        IActionResult salida;
        try
        {
            var eliminado = await _useCase.EliminarAsync(id);
            salida = Ok(ApiResponse<bool>.Ok(eliminado, "Equipo eliminado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }
}
