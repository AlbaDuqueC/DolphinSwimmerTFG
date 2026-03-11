namespace SwimmingApi.Application.Dtos.Equipo;

/// <summary>
/// DTO de respuesta con los datos de un equipo.
/// </summary>
public class EquipoResponseDto
{
    public int Id { get; set; }
    public int IdEquipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public int TotalNadadores { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
