using Microsoft.AspNetCore.Diagnostics;

namespace SwimmingApi.Api;

/// <summary>
/// Manejador global de excepciones no controladas.
/// Intercepta los errores y devuelve una respuesta JSON estandarizada.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Captura la excepción, la registra y devuelve una respuesta de error al cliente.
    /// </summary>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Error no controlado: {Mensaje}", exception.Message);

        var codigoEstado = exception switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

        httpContext.Response.StatusCode = codigoEstado;
        httpContext.Response.ContentType = "application/json";

        var respuesta = ApiResponse<object>.Error(exception.Message);
        await httpContext.Response.WriteAsJsonAsync(respuesta, cancellationToken);

        var resultado = true;
        return resultado;
    }
}
