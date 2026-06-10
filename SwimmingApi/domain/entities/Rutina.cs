namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa una rutina de entrenamiento o evento del calendario.
/// Puede pertenecer a un nadador (rutina personal) o a un entrenador,
/// en cuyo caso la rutina se replica automáticamente para todos los nadadores
/// del equipo que gestiona.
/// </summary>
public class Rutina : EntityBase
{
    /// <summary>Título corto del evento o rutina (ej. "Entrenamiento crol", "Competición Espartina").</summary>
    public string Titulo { get; set; } = string.Empty;

    /// <summary>Descripción detallada del evento: qué se va a hacer, qué pasó, notas del entrenador...</summary>
    public string? Descripcion { get; set; }

    /// <summary>Contenido heredado. Se mantiene por compatibilidad con datos existentes.
    /// Los nuevos registros usarán Titulo + Descripcion.</summary>
    public string Contenido { get; set; } = string.Empty;

    /// <summary>Fecha en la que se realizará o se realizó la rutina.</summary>
    public DateTime Fecha { get; set; }

    /// <summary>Indica si la rutina es visible para el usuario en su pantalla de inicio.</summary>
    public bool Mostrar { get; set; }

    /// <summary>Clave foránea del usuario al que pertenece la rutina.</summary>
    public int IdUsuario { get; set; }

    /// <summary>
    /// Propiedad de navegación al usuario propietario de la rutina.
    /// Entity Framework la carga automáticamente cuando se incluye en la consulta.
    /// </summary>
    public Usuario Usuario { get; set; } = null!;
}
