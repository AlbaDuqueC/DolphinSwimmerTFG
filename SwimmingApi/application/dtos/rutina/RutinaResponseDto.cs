namespace SwimmingApi.Application.Dtos.Rutina;

/// <summary>
/// DTO de salida con los datos de una rutina.
/// Es lo que la API devuelve al cliente al consultar rutinas.
/// </summary>
public class RutinaResponseDto
{
    /// <summary>Identificador interno del registro en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Identificador específico de la rutina en el dominio.</summary>
    public int IdRutina { get; set; }

    /// <summary>Título corto del evento.</summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>Descripción detallada del evento. Puede ser nula.</summary>
    public string? Descripcion { get; set; }

    /// <summary>Contenido heredado. Coincide con el Titulo en registros nuevos.</summary>
    public string Contenido { get; set; } = string.Empty;

    /// <summary>Fecha asignada a la rutina.</summary>
    public DateTime Fecha { get; set; }

    /// <summary>Indica si la rutina se muestra en la pantalla de inicio del usuario.</summary>
    public bool Mostrar { get; set; }

    /// <summary>ID del usuario al que pertenece la rutina.</summary>
    public int IdUsuario { get; set; }

    /// <summary>Fecha y hora en que se creó el registro.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha y hora de la última actualización (nula si nunca se ha modificado).</summary>
    public DateTime? UpdateAt { get; set; }
}
