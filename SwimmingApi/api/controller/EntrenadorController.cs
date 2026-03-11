using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Entrenador;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador para operaciones sobre Entrenadores.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EntrenadorController : ControllerBase
{
    private readonly IEntrenadorUseCase _useCase;

    public EntrenadorController(IEntrenadorUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene todos los entrenadores activos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EntrenadorResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var entrenadores = await _useCase.ObtenerTodosAsync();
        var resultado = Ok(ApiResponse<IEnumerable<EntrenadorResponseDto>>.Ok(entrenadores));
        return resultado;
    }

    /// <summary>Obtiene un entrenador por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EntrenadorResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var entrenador = await _useCase.ObtenerPorIdAsync(id);
        var resultado = entrenador != null
            ? Ok(ApiResponse<EntrenadorResponseDto>.Ok(entrenador))
            : NotFound(ApiResponse<EntrenadorResponseDto>.Error($"Entrenador con ID {id} no encontrado."));
        return resultado;
    }

    /// <summary>Crea un nuevo entrenador.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EntrenadorResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] EntrenadorRequestDto dto)
    {
        var entrenador = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = entrenador.Id },
            ApiResponse<EntrenadorResponseDto>.Ok(entrenador, "Entrenador creado con éxito."));
        return resultado;
    }

    /// <summary>Actualiza los datos de un entrenador.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EntrenadorResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EntrenadorRequestDto dto)
    {
        var entrenador = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<EntrenadorResponseDto>.Ok(entrenador, "Entrenador actualizado con éxito."));
        return resultado;
    }

    /// <summary>Elimina lógicamente un entrenador.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminado, "Entrenador eliminado con éxito."));
        return resultado;
    }
}
