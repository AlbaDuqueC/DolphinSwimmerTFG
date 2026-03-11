using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.NadadorEquipo;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador para operaciones sobre NadadorEquipo.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NadadorEquipoController : ControllerBase
{
    private readonly INadadorEquipoUseCase _useCase;

    public NadadorEquipoController(INadadorEquipoUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene todos los nadadores de un equipo.</summary>
    [HttpGet("equipo/{idEquipo:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NadadorEquipoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerPorEquipo(int idEquipo)
    {
        var lista = await _useCase.ObtenerPorEquipoAsync(idEquipo);
        var resultado = Ok(ApiResponse<IEnumerable<NadadorEquipoResponseDto>>.Ok(lista));
        return resultado;
    }

    /// <summary>Obtiene un NadadorEquipo por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var nadadorEquipo = await _useCase.ObtenerPorIdAsync(id);
        var resultado = nadadorEquipo != null
            ? Ok(ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo))
            : NotFound(ApiResponse<NadadorEquipoResponseDto>.Error($"NadadorEquipo con ID {id} no encontrado."));
        return resultado;
    }

    /// <summary>Obtiene un NadadorEquipo por su código de conexión.</summary>
    [HttpGet("codigo/{codigo:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorCodigo(int codigo)
    {
        var nadadorEquipo = await _useCase.ObtenerPorCodigoAsync(codigo);
        var resultado = nadadorEquipo != null
            ? Ok(ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo))
            : NotFound(ApiResponse<NadadorEquipoResponseDto>.Error($"No se encontró un nadador con el código {codigo}."));
        return resultado;
    }

    /// <summary>Crea un nuevo NadadorEquipo. Solo lo puede hacer un entrenador.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] NadadorEquipoRequestDto dto)
    {
        var nadadorEquipo = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = nadadorEquipo.Id },
            ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo, "NadadorEquipo creado con éxito."));
        return resultado;
    }

    /// <summary>Actualiza los datos de un NadadorEquipo.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] NadadorEquipoRequestDto dto)
    {
        var nadadorEquipo = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo, "NadadorEquipo actualizado con éxito."));
        return resultado;
    }

    /// <summary>Elimina lógicamente un NadadorEquipo.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminado, "NadadorEquipo eliminado con éxito."));
        return resultado;
    }
}
