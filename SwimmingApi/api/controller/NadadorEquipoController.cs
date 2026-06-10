using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.NadadorEquipo;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador REST para operaciones sobre NadadorEquipo.
/// Un NadadorEquipo representa una "plaza" dentro de un equipo, creada por el entrenador.
/// Cada plaza tiene un código único que el nadador real puede usar para vincularse.
/// Solo conoce la capa Application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NadadorEquipoController : ControllerBase
{
    // Caso de uso que contiene la lógica de negocio para NadadorEquipo.
    private readonly INadadorEquipoUseCase _useCase;

    /// <summary>
    /// Constructor con inyección de dependencias del caso de uso.
    /// </summary>
    public NadadorEquipoController(INadadorEquipoUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// Obtiene todos los nadadores registrados dentro de un equipo concreto.
    /// Es la consulta principal de la pantalla del equipo.
    /// </summary>
    [HttpGet("equipo/{idEquipo:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<NadadorEquipoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerPorEquipo(int idEquipo)
    {
        IActionResult salida;
        try
        {
            var lista = await _useCase.ObtenerPorEquipoAsync(idEquipo);
            if (!lista.Any())
            {
                salida = NoContent();
            }
            else
            {
                salida = Ok(ApiResponse<IEnumerable<NadadorEquipoResponseDto>>.Ok(lista));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene un NadadorEquipo por su ID.
    /// Devuelve 404 si no existe.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        IActionResult salida;
        try
        {
            var nadadorEquipo = await _useCase.ObtenerPorIdAsync(id);
            if (nadadorEquipo == null)
            {
                salida = NotFound(ApiResponse<NadadorEquipoResponseDto>.Error($"NadadorEquipo con ID {id} no encontrado."));
            }
            else
            {
                salida = Ok(ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene un NadadorEquipo a partir de su código único.
    /// Es la operación que ejecuta la app cuando un nadador
    /// introduce su código de 6 dígitos para unirse al equipo.
    /// </summary>
    [HttpGet("codigo/{codigo:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorCodigo(int codigo)
    {
        IActionResult salida;
        try
        {
            var nadadorEquipo = await _useCase.ObtenerPorCodigoAsync(codigo);
            if (nadadorEquipo == null)
            {
                salida = NotFound(ApiResponse<NadadorEquipoResponseDto>.Error($"No se encontró un nadador con el código {codigo}."));
            }
            else
            {
                salida = Ok(ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Crea un nuevo NadadorEquipo dentro de un equipo.
    /// Esta acción solo puede realizarla un entrenador.
    /// El sistema genera un código único de 6 dígitos automáticamente.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] NadadorEquipoRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var nadadorEquipo = await _useCase.CrearAsync(dto);
            salida = CreatedAtAction(nameof(ObtenerPorId), new { id = nadadorEquipo.Id },
                ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo, "NadadorEquipo creado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Actualiza los datos de un NadadorEquipo existente (nombre o apellidos).
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<NadadorEquipoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] NadadorEquipoRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var nadadorEquipo = await _useCase.ActualizarAsync(id, dto);
            salida = Ok(ApiResponse<NadadorEquipoResponseDto>.Ok(nadadorEquipo, "NadadorEquipo actualizado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Elimina lógicamente un NadadorEquipo del equipo.
    /// Si está vinculado a una cuenta de usuario real, esa cuenta queda desvinculada
    /// automáticamente y vuelve al estado "sin equipo".
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
            salida = Ok(ApiResponse<bool>.Ok(eliminado, "NadadorEquipo eliminado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }
}
