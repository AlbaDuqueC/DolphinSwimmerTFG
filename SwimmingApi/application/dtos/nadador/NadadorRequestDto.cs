namespace SwimmingApi.Application.Dtos.Nadador;

/// <summary>
/// DTO para crear o actualizar un nadador.
/// </summary>
public class NadadorRequestDto
{
    /// <summary>Nombre del nadador.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del nadador.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Email del nadador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña en texto plano (se encriptará antes de guardar).</summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>ID del equipo al que se une. Opcional.</summary>
    public int? IdEquipo { get; set; }

    /// <summary>Código de NadadorEquipo para vincular su perfil. Opcional.</summary>
    public int? CodigoNadadorEquipo { get; set; }
}
