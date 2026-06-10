using Microsoft.AspNetCore.Mvc;
using SwimmingApi.Application.Dtos.MarcaDeTiempo;
using SwimmingApi.Application.Interfaces.UseCase;

namespace SwimmingApi.Api.Controller;

/// <summary>
/// Controlador REST para operaciones sobre MarcasDeTiempo.
/// Una marca puede ser registrada por el propio nadador o por un entrenador
/// que asigna un tiempo a uno de los nadadores de su equipo.
/// Solo conoce la capa Application.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class MarcaDeTiempoController : ControllerBase
{
    // Caso de uso que contiene la lógica de negocio para marcas de tiempo.
    private readonly IMarcaDeTiempoUseCase _useCase;

    /// <summary>
    /// Constructor con inyección de dependencias del caso de uso.
    /// </summary>
    public MarcaDeTiempoController(IMarcaDeTiempoUseCase useCase)
    {
        _useCase = useCase;
    }

    /// <summary>
    /// Obtiene todas las marcas de tiempo asociadas a un NadadorEquipo concreto.
    /// Incluye tanto las marcas asignadas por el entrenador como las propias.
    /// </summary>
    [HttpGet("nadadorequipo/{idNadadorEquipo:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MarcaDeTiempoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerPorNadadorEquipo(int idNadadorEquipo)
    {
        IActionResult salida;
        try
        {
            var marcas = await _useCase.ObtenerPorNadadorEquipoAsync(idNadadorEquipo);
            if (!marcas.Any())
            {
                salida = NoContent();
            }
            else
            {
                salida = Ok(ApiResponse<IEnumerable<MarcaDeTiempoResponseDto>>.Ok(marcas));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene todas las marcas de tiempo registradas por un nadador.
    /// Sirve para mostrar al usuario sus propias marcas personales.
    /// </summary>
    [HttpGet("nadador/{idNadador:int}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<MarcaDeTiempoResponseDto>>), 200)]
    public async Task<IActionResult> ObtenerPorNadador(int idNadador)
    {
        IActionResult salida;
        try
        {
            var marcas = await _useCase.ObtenerPorNadadorAsync(idNadador);
            if (!marcas.Any())
            {
                salida = NoContent();
            }
            else
            {
                salida = Ok(ApiResponse<IEnumerable<MarcaDeTiempoResponseDto>>.Ok(marcas));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Obtiene una marca de tiempo concreta por su ID.
    /// Devuelve 404 si no existe.
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<MarcaDeTiempoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        IActionResult salida;
        try
        {
            var marca = await _useCase.ObtenerPorIdAsync(id);
            if (marca == null)
            {
                salida = NotFound(ApiResponse<MarcaDeTiempoResponseDto>.Error($"MarcaDeTiempo con ID {id} no encontrada."));
            }
            else
            {
                salida = Ok(ApiResponse<MarcaDeTiempoResponseDto>.Ok(marca));
            }
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Registra una nueva marca de tiempo asociada a un NadadorEquipo.
    /// Si IdNadador es nulo, significa que la marca la ha creado el entrenador.
    /// En caso contrario, la ha registrado el propio nadador.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<MarcaDeTiempoResponseDto>), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Crear([FromBody] MarcaDeTiempoRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var marca = await _useCase.CrearAsync(dto);
            salida = CreatedAtAction(nameof(ObtenerPorId), new { id = marca.Id },
                ApiResponse<MarcaDeTiempoResponseDto>.Ok(marca, "Marca de tiempo registrada con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Actualiza el tiempo o la descripción de una marca existente.
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ApiResponse<MarcaDeTiempoResponseDto>), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Actualizar(int id, [FromBody] MarcaDeTiempoRequestDto dto)
    {
        IActionResult salida;
        try
        {
            var marca = await _useCase.ActualizarAsync(id, dto);
            salida = Ok(ApiResponse<MarcaDeTiempoResponseDto>.Ok(marca, "Marca de tiempo actualizada con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }

    /// <summary>
    /// Elimina lógicamente una marca de tiempo.
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
            var eliminada = await _useCase.EliminarAsync(id);
            salida = Ok(ApiResponse<bool>.Ok(eliminada, "Marca de tiempo eliminada con éxito."));
        }
        catch
        {
            salida = BadRequest();
        }
        return salida;
    }
}
