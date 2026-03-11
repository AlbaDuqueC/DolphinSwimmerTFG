namespace SwimmingApi.Application.Dtos.NadadorEquipo;

/// <summary>
/// DTO de respuesta con los datos de un NadadorEquipo.
/// </summary>
public class NadadorEquipoResponseDto
{
    public int Id { get; set; }
    public int IdNadadorEquipo { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public int Codigo { get; set; }
    public int IdEquipo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
