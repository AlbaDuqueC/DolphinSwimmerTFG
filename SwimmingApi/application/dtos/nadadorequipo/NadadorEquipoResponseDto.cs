namespace SwimmingApi.Application.Dtos.NadadorEquipo;

/// <summary>
/// DTO de salida con los datos de un NadadorEquipo.
/// Es lo que la API devuelve al cliente al consultar nadadores de un equipo.
/// </summary>
public class NadadorEquipoResponseDto
{
    /// <summary>Identificador interno del registro en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Identificador específico del NadadorEquipo en el dominio.</summary>
    public int IdNadadorEquipo { get; set; }

    /// <summary>Nombre del nadador en el equipo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del nadador en el equipo.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Código único de 6 dígitos generado automáticamente.
    /// Lo usa el nadador real para vincular su cuenta a esta plaza del equipo.
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>ID del equipo al que pertenece.</summary>
    public int IdEquipo { get; set; }

    /// <summary>Fecha y hora en que se creó el registro.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha y hora de la última actualización (nula si nunca se ha modificado).</summary>
    public DateTime? UpdateAt { get; set; }
}