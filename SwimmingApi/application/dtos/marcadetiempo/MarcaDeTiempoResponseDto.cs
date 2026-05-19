namespace SwimmingApi.Application.Dtos.MarcaDeTiempo;

/// <summary>
/// DTO de salida con los datos de una marca de tiempo.
/// Es lo que la API devuelve al cliente al consultar marcas.
/// </summary>
public class MarcaDeTiempoResponseDto
{
    /// <summary>Identificador interno del registro en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Identificador específico de la marca en el dominio.</summary>
    public int IdMarca { get; set; }

    /// <summary>Tiempo registrado en formato "hh:mm:ss.fff".</summary>
    public TimeSpan Tiempo { get; set; }

    /// <summary>Descripción de la prueba (por ejemplo: "100m libre").</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>ID del NadadorEquipo al que pertenece la marca.</summary>
    public int? IdNadadorEquipo { get; set; }

    /// <summary>
    /// ID del nadador que registró la marca.
    /// Si es nulo, la marca la asignó el entrenador.
    /// </summary>
    public int? IdNadador { get; set; }

    /// <summary>Fecha y hora en que se registró la marca.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha y hora de la última actualización (nula si nunca se ha modificado).</summary>
    public DateTime? UpdateAt { get; set; }
}