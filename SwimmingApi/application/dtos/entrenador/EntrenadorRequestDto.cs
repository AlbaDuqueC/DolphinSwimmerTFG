namespace SwimmingApi.Application.Dtos.Entrenador;

/// <summary>
/// DTO para crear o actualizar un entrenador.
/// </summary>
public class EntrenadorRequestDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int? IdEquipoGestionado { get; set; }
}
