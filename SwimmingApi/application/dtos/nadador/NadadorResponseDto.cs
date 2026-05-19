namespace SwimmingApi.Application.Dtos.Nadador;

/// <summary>
/// DTO de salida con los datos públicos de un nadador.
/// Es lo que la API devuelve al cliente, sin información sensible como la contraseña.
/// </summary>
public class NadadorResponseDto
{
    /// <summary>Identificador interno del registro en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Identificador específico del nadador en el dominio.</summary>
    public int IdNadador { get; set; }

    /// <summary>Nombre del nadador.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del nadador.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Correo electrónico del nadador.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>ID del equipo al que pertenece (nulo si todavía no se ha unido a ninguno).</summary>
    public int? IdEquipo { get; set; }

    /// <summary>ID del NadadorEquipo al que está vinculado dentro del equipo.</summary>
    public int? IdNadadorEquipo { get; set; }

    /// <summary>Fecha y hora en que se creó el registro.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha y hora de la última actualización (nula si nunca se ha modificado).</summary>
    public DateTime? UpdateAt { get; set; }
}