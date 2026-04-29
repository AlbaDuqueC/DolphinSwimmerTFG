namespace SwimmingApi.Application.Dtos.MarcaDeTiempo;

/// <summary>
/// DTO para registrar una marca de tiempo.
/// </summary>
public class MarcaDeTiempoRequestDto
{
    /// <summary>Tiempo registrado en formato "hh:mm:ss.fff".</summary>
    public TimeSpan Tiempo { get; set; }

    /// <summary>Descripción de la prueba (ej: "100m libre").</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>ID del NadadorEquipo al que se asigna la marca.</summary>
    public int? IdNadadorEquipo { get; set; }

    /// <summary>ID del nadador que registra. Nulo si lo registra el entrenador.</summary>
    public int? IdNadador { get; set; }
}
