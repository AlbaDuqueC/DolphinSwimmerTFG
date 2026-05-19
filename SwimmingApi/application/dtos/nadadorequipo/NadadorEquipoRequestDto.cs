namespace SwimmingApi.Application.Dtos.NadadorEquipo;

/// <summary>
/// DTO de entrada para crear o actualizar un NadadorEquipo.
/// Representa una plaza de nadador dentro de un equipo, creada por el entrenador.
/// </summary>
public class NadadorEquipoRequestDto
{
    /// <summary>Nombre del nadador en el equipo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del nadador en el equipo.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>ID del equipo al que pertenece. Obligatorio.</summary>
    public int IdEquipo { get; set; }
}