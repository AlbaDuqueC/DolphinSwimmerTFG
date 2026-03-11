namespace SwimmingApi.Application.Dtos.Equipo;

/// <summary>
/// DTO para crear o actualizar un equipo.
/// </summary>
public class EquipoRequestDto
{
    /// <summary>Nombre del equipo.</summary>
    public string Nombre { get; set; } = string.Empty;
}
