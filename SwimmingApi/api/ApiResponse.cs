namespace SwimmingApi.Api;

/// <summary>
/// Envoltorio estándar para todas las respuestas de la API.
/// Garantiza que cada endpoint devuelva un formato JSON consistente,
/// lo que simplifica el tratamiento de respuestas en el cliente.
/// </summary>
/// <typeparam name="T">Tipo de los datos devueltos en la respuesta.</typeparam>
public class ApiResponse<T>
{
    /// <summary>Indica si la operación se completó correctamente.</summary>
    public bool Exito { get; set; }

    /// <summary>Mensaje descriptivo del resultado (éxito o error).</summary>
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>Datos devueltos por la operación. Es nulo en caso de error.</summary>
    public T? Datos { get; set; }

    /// <summary>
    /// Crea una respuesta exitosa que incluye los datos y un mensaje opcional.
    /// </summary>
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

    /// <summary>
    /// Crea una respuesta de error sin datos.
    /// </summary>
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