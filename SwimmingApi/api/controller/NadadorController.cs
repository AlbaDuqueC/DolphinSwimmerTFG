using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Nadador;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

[ApiController]
[Route("api/[controller]")]
public class NadadorController : ControllerBase
{
    private readonly INadadorUseCase _useCase;

    public NadadorController(INadadorUseCase useCase)
    {
        _useCase = useCase;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        var nadadores = await _useCase.ObtenerTodosAsync();
        var resultado = Ok(ApiResponse<IEnumerable<NadadorResponseDto>>.Ok(nadadores));
        return resultado;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var nadador = await _useCase.ObtenerPorIdAsync(id);
        IActionResult resultado = nadador != null
            ? Ok(ApiResponse<NadadorResponseDto>.Ok(nadador))
            : NotFound(ApiResponse<NadadorResponseDto>.Error($"Nadador con ID {id} no encontrado."));
        return resultado;
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] NadadorRequestDto dto)
    {
        var nadador = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = nadador.Id },
            ApiResponse<NadadorResponseDto>.Ok(nadador, "Nadador creado con éxito."));
        return resultado;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] NadadorRequestDto dto)
    {
        var nadador = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<NadadorResponseDto>.Ok(nadador, "Nadador actualizado con éxito."));
        return resultado;
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminado, "Nadador eliminado con éxito."));
        return resultado;
    }
}