namespace SwimmingApi.Application.Dtos.Nadador;

/// <summary>
/// DTO de respuesta con los datos de un nadador.
/// </summary>
public class NadadorResponseDto
{
    public int Id { get; set; }
    public int IdNadador { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? IdEquipo { get; set; }
    public int? IdNadadorEquipo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
