namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa una rutina de entrenamiento.
/// Puede pertenecer a un nadador o a un entrenador para su equipo.
/// </summary>
public class Rutina : EntityBase
{

    /// <summary>Contenido detallado de la rutina.</summary>
    public string Contenido { get; set; } = string.Empty;

    /// <summary>Fecha en la que se realizará o realizó la rutina.</summary>
    public DateTime Fecha { get; set; }

    /// <summary>Indica si la rutina es visible para otros usuarios.</summary>
    public bool Mostrar { get; set; }

    /// <summary>FK del usuario al que pertenece la rutina.</summary>
    public int IdUsuario { get; set; }

    /// <summary>Usuario propietario de la rutina.</summary>
    public Usuario Usuario { get; set; } = null!;
}
