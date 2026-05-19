using Microsoft.AspNetCore.Diagnostics;

namespace SwimmingApi.Api;

/// <summary>
/// Manejador global de excepciones no controladas.
/// Intercepta cualquier error lanzado en la API, lo registra en el log
/// y devuelve al cliente una respuesta JSON estandarizada en lugar de
/// exponer detalles internos del servidor.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    // Servicio de logging para registrar los errores ocurridos.
    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <summary>
    /// Constructor con inyección de dependencias del logger.
    /// </summary>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Captura la excepción producida, la registra en el log
    /// y construye una respuesta de error apropiada para devolver al cliente.
    /// El código HTTP varía en función del tipo de excepción.
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Se registra el error en el log para poder revisarlo después.
        _logger.LogError(exception, "Error no controlado: {Mensaje}", exception.Message);

        // Se determina el código HTTP de la respuesta según el tipo de excepción.
        var codigoEstado = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        // Se configura la respuesta HTTP con el código y el formato JSON.
        httpContext.Response.StatusCode = codigoEstado;
        httpContext.Response.ContentType = "application/json";

        // Se envía al cliente la respuesta estandarizada con el mensaje del error.
        var respuesta = ApiResponse<object>.Error(exception.Message);
        await httpContext.Response.WriteAsJsonAsync(respuesta, cancellationToken);

        // Devuelve true para indicar que la excepción ya ha sido manejada.
        var resultado = true;
        return resultado;
    }
}