namespace SwimmingApi.Application.Dtos.Entrenador;

/// <summary>
/// DTO de entrada para crear o actualizar un entrenador.
/// Recoge los datos que se reciben en el cuerpo de la petición HTTP.
/// </summary>
public class EntrenadorRequestDto
{
    /// <summary>Nombre del entrenador.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del entrenador.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Correo electrónico del entrenador. Sirve para identificarlo en el login.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña en texto plano (se encripta antes de guardarla).</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>ID del equipo que gestiona. Opcional al registrarse.</summary>
    public int? IdEquipoGestionado { get; set; }
}