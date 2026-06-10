using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Rutina;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador REST para operaciones sobre Rutinas.
/// Las rutinas son notas personales que un usuario (nadador o entrenador)
/// crea para organizar sus entrenamientos o recordatorios.
/// Solo conoce la capa Application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RutinaController : ControllerBase
{
    // Caso de uso que contiene la lógica de negocio para rutinas.
    private readonly IRutinaUseCase _useCase;

    /// <summary>
    /// Constructor con inyección de dependencias del caso de uso.
    /// </summary>
    public RutinaController(IRutinaUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// Obtiene todas las rutinas asociadas a un usuario concreto.
    /// Sirve para mostrar al usuario sus propias rutinas en la pantalla de inicio.
    /// </summary>
    [HttpGet("usuario/{idUsuario:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RutinaResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerPorUsuario(int idUsuario)
    {
        IActionResult salida;
        try
        {
            var rutinas = await _useCase.ObtenerPorUsuarioAsync(idUsuario);
            if (!rutinas.Any())
            {
                salida = NoContent();
            }
            else
            {
                salida = Ok(ApiResponse<IEnumerable<RutinaResponseDto>>.Ok(rutinas));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene una rutina concreta por su ID.
    /// Devuelve 404 si no existe.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RutinaResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        IActionResult salida;
        try
        {
            var rutina = await _useCase.ObtenerPorIdAsync(id);
            if (rutina == null)
            {
                salida = NotFound(ApiResponse<RutinaResponseDto>.Error($"Rutina con ID {id} no encontrada."));
            }
            else
            {
                salida = Ok(ApiResponse<RutinaResponseDto>.Ok(rutina));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Crea una nueva rutina para un usuario.
    /// Recibe el contenido y la fecha de la rutina en el cuerpo de la petición.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RutinaResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] RutinaRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var rutina = await _useCase.CrearAsync(dto);
            salida = CreatedAtAction(nameof(ObtenerPorId), new { id = rutina.Id },
                ApiResponse<RutinaResponseDto>.Ok(rutina, "Rutina creada con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Actualiza el contenido o la fecha de una rutina existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RutinaResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] RutinaRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var rutina = await _useCase.ActualizarAsync(id, dto);
            salida = Ok(ApiResponse<RutinaResponseDto>.Ok(rutina, "Rutina actualizada con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Elimina lógicamente una rutina del sistema.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        IActionResult salida;
        try
        {
            var eliminada = await _useCase.EliminarAsync(id);
            salida = Ok(ApiResponse<bool>.Ok(eliminada, "Rutina eliminada con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }
}
