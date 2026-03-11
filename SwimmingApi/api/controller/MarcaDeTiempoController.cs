using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.MarcaDeTiempo;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador para operaciones sobre MarcasDeTiempo.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MarcaDeTiempoController : ControllerBase
{
    private readonly IMarcaDeTiempoUseCase _useCase;

    public MarcaDeTiempoController(IMarcaDeTiempoUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene todas las marcas de tiempo de un NadadorEquipo.</summary>
    [HttpGet("nadadorequipo/{idNadadorEquipo:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MarcaDeTiempoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerPorNadadorEquipo(int idNadadorEquipo)
    {
        var marcas = await _useCase.ObtenerPorNadadorEquipoAsync(idNadadorEquipo);
        var resultado = Ok(ApiResponse<IEnumerable<MarcaDeTiempoResponseDto>>.Ok(marcas));
        return resultado;
    }

    /// <summary>Obtiene una marca de tiempo por su ID.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<MarcaDeTiempoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var marca = await _useCase.ObtenerPorIdAsync(id);
        var resultado = marca != null
            ? Ok(ApiResponse<MarcaDeTiempoResponseDto>.Ok(marca))
            : NotFound(ApiResponse<MarcaDeTiempoResponseDto>.Error($"MarcaDeTiempo con ID {id} no encontrada."));
        return resultado;
    }

    /// <summary>Registra una nueva marca de tiempo.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MarcaDeTiempoResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] MarcaDeTiempoRequestDto dto)
    {
        var marca = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = marca.Id },
            ApiResponse<MarcaDeTiempoResponseDto>.Ok(marca, "Marca de tiempo registrada con éxito."));
        return resultado;
    }

    /// <summary>Actualiza una marca de tiempo existente.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<MarcaDeTiempoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] MarcaDeTiempoRequestDto dto)
    {
        var marca = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<MarcaDeTiempoResponseDto>.Ok(marca, "Marca de tiempo actualizada con éxito."));
        return resultado;
    }

    /// <summary>Elimina lógicamente una marca de tiempo.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminada = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminada, "Marca de tiempo eliminada con éxito."));
        return resultado;
    }
}
