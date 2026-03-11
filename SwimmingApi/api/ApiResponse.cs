namespace SwimmingApi.Api;

/// <summary>
/// Envoltorio estándar para todas las respuestas de la API.
/// Garantiza un formato consistente en cada endpoint.
/// </summary>
public class ApiResponse<T>
{
    /// <summary>Indica si la operación fue exitosa.</summary>
    public bool Exito { get; set; }

    /// <summary>Mensaje descriptivo del resultado.</summary>
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>Datos devueltos por la operación. Puede ser nulo en errores.</summary>
    public T? Datos { get; set; }

    /// <summary>Crea una respuesta exitosa con datos.</summary>
    public static ApiResponse<T> Ok(T datos, string mensaje = "Operación completada con éxito.")
    {
        var resultado = new ApiResponse<T>
        {
            Exito = true,
            Mensaje = mensaje,
            Datos = datos
        };
        return resultado;
    }

    /// <summary>Crea una respuesta de error sin datos.</summary>
    public static ApiResponse<T> Error(string mensaje)
    {
        var resultado = new ApiResponse<T>
        {
            Exito = false,
            Mensaje = mensaje,
            Datos = default
        };
        return resultado;
    }
}
