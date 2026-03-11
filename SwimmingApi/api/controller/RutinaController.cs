using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Rutina;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador para operaciones sobre Rutinas.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RutinaController : ControllerBase
{
    private readonly IRutinaUseCase _useCase;

    public RutinaController(IRutinaUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene todas las rutinas de un usuario.</summary>
    [HttpGet("usuario/{idUsuario:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RutinaResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerPorUsuario(int idUsuario)
    {
        var rutinas = await _useCase.ObtenerPorUsuarioAsync(idUsuario);
        var resultado = Ok(ApiResponse<IEnumerable<RutinaResponseDto>>.Ok(rutinas));
        return resultado;
    }

    /// <summary>Obtiene una rutina por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RutinaResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var rutina = await _useCase.ObtenerPorIdAsync(id);
        var resultado = rutina != null
            ? Ok(ApiResponse<RutinaResponseDto>.Ok(rutina))
            : NotFound(ApiResponse<RutinaResponseDto>.Error($"Rutina con ID {id} no encontrada."));
        return resultado;
    }

    /// <summary>Crea una nueva rutina.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<RutinaResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] RutinaRequestDto dto)
    {
        var rutina = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = rutina.Id },
            ApiResponse<RutinaResponseDto>.Ok(rutina, "Rutina creada con éxito."));
        return resultado;
    }

    /// <summary>Actualiza una rutina existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<RutinaResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] RutinaRequestDto dto)
    {
        var rutina = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<RutinaResponseDto>.Ok(rutina, "Rutina actualizada con éxito."));
        return resultado;
    }

    /// <summary>Elimina lógicamente una rutina.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminada = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminada, "Rutina eliminada con éxito."));
        return resultado;
    }
}
