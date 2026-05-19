namespace SwimmingApi.Application.Dtos.Equipo;

/// <summary>
/// DTO de salida con los datos de un equipo.
/// Es lo que la API devuelve al cliente al consultar equipos.
/// </summary>
public class EquipoResponseDto
{
    /// <summary>Identificador interno del registro en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Identificador específico del equipo en el dominio.</summary>
    public int IdEquipo { get; set; }

    /// <summary>Nombre del equipo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Número total de nadadores que pertenecen al equipo.</summary>
    public int TotalNadadores { get; set; }

    /// <summary>Fecha y hora en que se creó el equipo.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha y hora de la última actualización (nula si nunca se ha modificado).</summary>
    public DateTime? UpdateAt { get; set; }
}