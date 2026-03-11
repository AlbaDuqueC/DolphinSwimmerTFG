using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Equipo;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador para operaciones sobre Equipos.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EquipoController : ControllerBase
{
    private readonly IEquipoUseCase _useCase;

    public EquipoController(IEquipoUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene todos los equipos activos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EquipoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerTodos()
    {
        var equipos = await _useCase.ObtenerTodosAsync();
        var resultado = Ok(ApiResponse<IEnumerable<EquipoResponseDto>>.Ok(equipos));
        return resultado;
    }

    /// <summary>Obtiene un equipo por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var equipo = await _useCase.ObtenerPorIdAsync(id);
        var resultado = equipo != null
            ? Ok(ApiResponse<EquipoResponseDto>.Ok(equipo))
            : NotFound(ApiResponse<EquipoResponseDto>.Error($"Equipo con ID {id} no encontrado."));
        return resultado;
    }

    /// <summary>Crea un nuevo equipo.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EquipoResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] EquipoRequestDto dto)
    {
        var equipo = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = equipo.Id },
            ApiResponse<EquipoResponseDto>.Ok(equipo, "Equipo creado con éxito."));
        return resultado;
    }

    /// <summary>Actualiza el nombre de un equipo.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EquipoRequestDto dto)
    {
        var equipo = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<EquipoResponseDto>.Ok(equipo, "Equipo actualizado con éxito."));
        return resultado;
    }

    /// <summary>Elimina lógicamente un equipo.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminado, "Equipo eliminado con éxito."));
        return resultado;
    }
}
