namespace SwimmingApi.Application.Dtos.MarcaDeTiempo;

/// <summary>
/// DTO de entrada para registrar una marca de tiempo.
/// Una marca puede ser registrada por el propio nadador o asignada por un entrenador.
/// </summary>
public class MarcaDeTiempoRequestDto
{
    /// <summary>Tiempo registrado en formato "hh:mm:ss.fff".</summary>
    public TimeSpan Tiempo { get; set; }

    /// <summary>Descripción de la prueba (por ejemplo: "100m libre").</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>ID del NadadorEquipo al que se asigna la marca.</summary>
    public int? IdNadadorEquipo { get; set; }

    /// <summary>
    /// ID del nadador que registra la marca.
    /// Si es nulo, significa que la marca la ha registrado el entrenador.
    /// </summary>
    public int? IdNadador { get; set; }
}