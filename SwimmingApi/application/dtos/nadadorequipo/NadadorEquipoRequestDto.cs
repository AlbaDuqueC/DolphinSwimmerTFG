namespace SwimmingApi.Application.Dtos.NadadorEquipo;

/// <summary>
/// DTO para crear o actualizar un NadadorEquipo.
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
