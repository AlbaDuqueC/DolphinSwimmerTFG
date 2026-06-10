using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.Entrenador;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador REST para operaciones sobre Entrenadores.
/// Recibe las peticiones HTTP del cliente y delega la lógica de negocio
/// al caso de uso correspondiente. Solo conoce la capa Application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class EntrenadorController : ControllerBase
{
    // Caso de uso que contiene la lógica de negocio para entrenadores.
    private readonly IEntrenadorUseCase _useCase;

    /// <summary>
    /// Constructor con inyección de dependencias del caso de uso.
    /// </summary>
    public EntrenadorController(IEntrenadorUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>Obtiene la lista de todos los entrenadores activos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EntrenadorResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerTodos()
    {
        IActionResult salida;
        try
        {
            var entrenadores = await _useCase.ObtenerTodosAsync();
            if (!entrenadores.Any())
            {
                salida = NoContent();
            }
            else
            {
                salida = Ok(ApiResponse<IEnumerable<EntrenadorResponseDto>>.Ok(entrenadores));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene un entrenador por su ID.
    /// Devuelve 404 si no existe.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EntrenadorResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        IActionResult salida;
        try
        {
            var entrenador = await _useCase.ObtenerPorIdAsync(id);
            if (entrenador == null)
            {
                salida = NotFound(ApiResponse<EntrenadorResponseDto>.Error($"Entrenador con ID {id} no encontrado."));
            }
            else
            {
                salida = Ok(ApiResponse<EntrenadorResponseDto>.Ok(entrenador));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene un entrenador a partir de su correo electrónico.
    /// Utilizado principalmente al iniciar sesión para identificar al usuario.
    /// </summary>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(ApiResponse<EntrenadorResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorEmail(string email)
    {
        IActionResult salida;
        try
        {
            var entrenador = await _useCase.ObtenerPorEmailAsync(email);
            if (entrenador == null)
            {
                salida = NotFound(ApiResponse<EntrenadorResponseDto>.Error($"No se encontró ningún entrenador con email {email}."));
            }
            else
            {
                salida = Ok(ApiResponse<EntrenadorResponseDto>.Ok(entrenador));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Crea un nuevo entrenador en el sistema.
    /// Recibe los datos del nuevo usuario en el cuerpo de la petición.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EntrenadorResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] EntrenadorRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var entrenador = await _useCase.CrearAsync(dto);
            salida = CreatedAtAction(nameof(ObtenerPorId), new { id = entrenador.Id },
                ApiResponse<EntrenadorResponseDto>.Ok(entrenador, "Entrenador creado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Actualiza los datos de un entrenador existente (nombre, apellidos, etc.).
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<EntrenadorResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EntrenadorRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var entrenador = await _useCase.ActualizarAsync(id, dto);
            salida = Ok(ApiResponse<EntrenadorResponseDto>.Ok(entrenador, "Entrenador actualizado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Elimina lógicamente un entrenador del sistema.
    /// El registro permanece en la base de datos pero se marca como inactivo.
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
            salida = Ok(ApiResponse<bool>.Ok(eliminado, "Entrenador eliminado con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }
}
