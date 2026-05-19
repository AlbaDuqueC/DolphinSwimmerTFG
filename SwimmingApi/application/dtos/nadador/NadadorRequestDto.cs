namespace SwimmingApi.Application.Dtos.Nadador;

/// <summary>
/// DTO de entrada para crear o actualizar un nadador.
/// Recoge los datos que se reciben en el cuerpo de la petición HTTP.
/// </summary>
public class NadadorRequestDto
{
    /// <summary>Nombre del nadador.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del nadador.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Correo electrónico del nadador. Sirve para identificarlo en el login.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña en texto plano (se encripta antes de guardarla).</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>ID del equipo al que pertenece. Opcional al registrarse.</summary>
    public int? IdEquipo { get; set; }

    /// <summary>
    /// Código de 6 dígitos del NadadorEquipo al que se quiere vincular.
    /// Opcional, solo se usa al unirse a un equipo.
    /// </summary>
    public int? CodigoNadadorEquipo { get; set; }
}