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
        var equipos = await _useCase.ObtenerTodosAsync();
        var resultado = Ok(ApiResponse<IEnumerable<EquipoResponseDto>>.Ok(equipos));
        return resultado;
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
        var equipo = await _useCase.ObtenerPorIdAsync(id);
        IActionResult resultado = equipo != null
            ? Ok(ApiResponse<EquipoResponseDto>.Ok(equipo))
            : NotFound(ApiResponse<EquipoResponseDto>.Error($"Equipo con ID {id} no encontrado."));
        return resultado;
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
        var equipo = await _useCase.CrearAsync(dto);
        var resultado = CreatedAtAction(nameof(ObtenerPorId), new { id = equipo.Id },
            ApiResponse<EquipoResponseDto>.Ok(equipo, "Equipo creado con éxito."));
        return resultado;
    }

    /// <summary>
    /// Actualiza el nombre de un equipo existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EquipoRequestDto dto)
    {
        var equipo = await _useCase.ActualizarAsync(id, dto);
        var resultado = Ok(ApiResponse<EquipoResponseDto>.Ok(equipo, "Equipo actualizado con éxito."));
        return resultado;
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
        var eliminado = await _useCase.EliminarAsync(id);
        var resultado = Ok(ApiResponse<bool>.Ok(eliminado, "Equipo eliminado con éxito."));
        return resultado;
    }
}