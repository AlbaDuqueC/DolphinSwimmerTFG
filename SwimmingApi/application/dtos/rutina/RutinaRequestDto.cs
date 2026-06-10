namespace SwimmingApi.Application.Dtos.Rutina;

/// <summary>
/// DTO de entrada para crear o actualizar una rutina.
/// Recoge los datos que se reciben en el cuerpo de la petición HTTP.
/// </summary>
public class RutinaRequestDto
{
    /// <summary>Título corto del evento (ej. "Entrenamiento crol", "Competición Espartina").</summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>Descripción detallada del evento. Opcional.</summary>
    public string? Descripcion { get; set; }

    /// <summary>Contenido heredado. Se rellena con el Titulo para compatibilidad con datos existentes.</summary>
    public string Contenido { get; set; } = string.Empty;

    /// <summary>Fecha asignada a la rutina.</summary>
    public DateTime Fecha { get; set; }

    /// <summary>Indica si la rutina se muestra en la pantalla de inicio del usuario.</summary>
    public bool Mostrar { get; set; }

    /// <summary>ID del usuario al que pertenece la rutina.</summary>
    public int IdUsuario { get; set; }
}
