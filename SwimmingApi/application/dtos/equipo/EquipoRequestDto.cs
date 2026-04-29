namespace SwimmingApi.Application.Dtos.Equipo;

/// <summary>
/// DTO para crear o actualizar un equipo.
/// </summary>
public class EquipoRequestDto
{
    /// <summary>Nombre del equipo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>ID del entrenador que lo crea. Si viene, se vincula como su equipo gestionado.</summary>
    public int? IdEntrenador { get; set; }
}