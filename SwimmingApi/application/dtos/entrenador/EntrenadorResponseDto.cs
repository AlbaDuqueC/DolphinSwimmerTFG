namespace SwimmingApi.Application.Dtos.Entrenador;

/// <summary>
/// DTO de salida con los datos públicos de un entrenador.
/// Es lo que la API devuelve al cliente, sin información sensible como la contraseña.
/// </summary>
public class EntrenadorResponseDto
{
    /// <summary>Identificador interno del registro en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Identificador específico del entrenador en el dominio.</summary>
    public int IdEntrenador { get; set; }

    /// <summary>Nombre del entrenador.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del entrenador.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Correo electrónico del entrenador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>URL de la foto de perfil almacenada en Firebase Storage. Nula si no tiene foto.</summary>
    public string? FotoPerfil { get; set; }

    /// <summary>ID del equipo al que pertenece (puede ser nulo).</summary>
    public int? IdEquipo { get; set; }

    /// <summary>ID del equipo que gestiona como entrenador.</summary>
    public int? IdEquipoGestionado { get; set; }

    /// <summary>Fecha y hora en que se creó el registro.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha y hora de la última actualización (nula si nunca se ha modificado).</summary>
    public DateTime? UpdateAt { get; set; }
}
