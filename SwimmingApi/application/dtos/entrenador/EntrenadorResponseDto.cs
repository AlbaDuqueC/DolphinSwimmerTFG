namespace SwimmingApi.Application.Dtos.Entrenador;

/// <summary>
/// DTO de respuesta con los datos de un entrenador.
/// </summary>
public class EntrenadorResponseDto
{
    public int Id { get; set; }
    public int IdEntrenador { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? IdEquipo { get; set; }
    public int? IdEquipoGestionado { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdateAt { get; set; }
}
