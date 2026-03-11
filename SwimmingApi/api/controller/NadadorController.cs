using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Nadador;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador para operaciones sobre Nadadores.
/// Solo conoce la capa Application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NadadorController : ControllerBase
{
    private readonly INadadorUseCase _useCase;

    public NadadorController(INadadorUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene todos los nadadores activos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NadadorResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var nadadores = await _useCase.ObtenerTodosAsync();
        var resultado = Ok(ApiResponse<IEnumerable<NadadorResponseDto>>.Ok(nadadores));
        return resultado;
    }

    /// <summary>Obtiene un nadador por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var nadador = await _useCase.ObtenerPorIdAsync(id);
        var resultado = nadador != null
            ? Ok(ApiResponse<NadadorResponseDto>.Ok(nadador))
            : NotFound(ApiResponse<NadadorResponseDto>.Error($"Nadador con ID {id} no encontrado."));
        return resultado;
    }

    /// <summary>Crea un nuevo nadador.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NadadorResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] NadadorRequestDto dto)
    {
        var nadador = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = nadador.Id },
            ApiResponse<NadadorResponseDto>.Ok(nadador, "Nadador creado con éxito."));
        return resultado;
    }

    /// <summary>Actualiza los datos de un nadador.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] NadadorRequestDto dto)
    {
        var nadador = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<NadadorResponseDto>.Ok(nadador, "Nadador actualizado con éxito."));
        return resultado;
    }

    /// <summary>Elimina lógicamente un nadador (no se borra de la base de datos).</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminado, "Nadador eliminado con éxito."));
        return resultado;
    }
}
