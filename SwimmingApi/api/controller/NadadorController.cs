using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Nadador;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// DTO auxiliar para recibir el código que un nadador introduce
/// al intentar unirse a un equipo a través de su entrenador.
/// </summary>
public class VincularCodigoRequest
{
    public int Codigo { get; set; }
}

/// <summary>
/// Controlador REST para operaciones sobre Nadadores.
/// Gestiona el ciclo de vida del nadador y su vinculación con equipos.
/// Solo conoce la capa Application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NadadorController : ControllerBase
{
    // Caso de uso que contiene la lógica de negocio para nadadores.
    private readonly INadadorUseCase _useCase;

    /// <summary>
    /// Constructor con inyección de dependencias del caso de uso.
    /// </summary>
    public NadadorController(INadadorUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene la lista de todos los nadadores activos del sistema.</summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerTodos()
    {
        IActionResult salida;
        try
        {
            var nadadores = await _useCase.ObtenerTodosAsync();
            if (!nadadores.Any())
            {
                salida = NoContent();
            }
            else
            {
                salida = Ok(ApiResponse<IEnumerable<NadadorResponseDto>>.Ok(nadadores));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene un nadador por su ID.
    /// Devuelve 404 si no existe.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        IActionResult salida;
        try
        {
            var nadador = await _useCase.ObtenerPorIdAsync(id);
            if (nadador == null)
            {
                salida = NotFound(ApiResponse<NadadorResponseDto>.Error($"Nadador con ID {id} no encontrado."));
            }
            else
            {
                salida = Ok(ApiResponse<NadadorResponseDto>.Ok(nadador));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene un nadador a partir de su correo electrónico.
    /// Utilizado principalmente al iniciar sesión para identificar al usuario.
    /// </summary>
    [HttpGet("email/{email}")]
    public async Task<IActionResult> ObtenerPorEmail(string email)
    {
        IActionResult salida;
        try
        {
            var nadador = await _useCase.ObtenerPorEmailAsync(email);
            if (nadador == null)
            {
                salida = NotFound(ApiResponse<NadadorResponseDto>.Error($"No se encontró ningún nadador con email {email}."));
            }
            else
            {
                salida = Ok(ApiResponse<NadadorResponseDto>.Ok(nadador));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Crea un nuevo nadador en el sistema.
    /// Recibe los datos del nuevo usuario en el cuerpo de la petición.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] NadadorRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var nadador = await _useCase.CrearAsync(dto);
            salida = CreatedAtAction(nameof(ObtenerPorId), new { id = nadador.Id },
                ApiResponse<NadadorResponseDto>.Ok(nadador, "Nadador creado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Actualiza los datos de un nadador existente (nombre, apellidos, etc.).
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] NadadorRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var nadador = await _useCase.ActualizarAsync(id, dto);
            salida = Ok(ApiResponse<NadadorResponseDto>.Ok(nadador, "Nadador actualizado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Elimina lógicamente un nadador del sistema.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        IActionResult salida;
        try
        {
            var eliminado = await _useCase.EliminarAsync(id);
            salida = Ok(ApiResponse<bool>.Ok(eliminado, "Nadador eliminado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Vincula un nadador con un NadadorEquipo del equipo
    /// utilizando el código único que le proporciona su entrenador.
    /// Si el código no existe devuelve 404. Si ya está vinculado devuelve 400.
    /// </summary>
    [HttpPost("{id:int}/vincular")]
    [ProducesResponseType(typeof(ApiResponse<NadadorResponseDto>), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> VincularConCodigo(int id, [FromBody] VincularCodigoRequest request)
    {
        IActionResult salida;
        try
        {
            var nadador = await _useCase.VincularConCodigoAsync(id, request.Codigo);
            salida = Ok(ApiResponse<NadadorResponseDto>.Ok(nadador, "Te has unido al equipo correctamente."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }
}
