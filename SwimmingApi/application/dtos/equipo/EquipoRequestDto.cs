namespace SwimmingApi.Application.Dtos.Equipo;

/// <summary>
/// DTO de entrada para crear o actualizar un equipo.
/// Recoge los datos que se reciben en el cuerpo de la petición HTTP.
/// </summary>
public class EquipoRequestDto
{
    /// <summary>Nombre del equipo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// ID del entrenador que crea el equipo.
    /// Si viene rellenado, se vincula como su equipo gestionado.
    /// </summary>
    public int? IdEntrenador { get; set; }
}