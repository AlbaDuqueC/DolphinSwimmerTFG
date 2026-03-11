namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad base de la que heredan todas las entidades del sistema.
/// Incluye campos de auditoría y eliminación lógica.
/// </summary>
public abstract class EntityBase
{
    /// <summary>Identificador único de la entidad.</summary>
    public int Id { get; set; }

    /// <summary>Fecha en la que fue creada la entidad.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>Fecha de última modificación. Puede ser nula si nunca se modificó.</summary>
    public DateTime? UpdateAt { get; set; }

    /// <summary>
    /// Fecha de eliminación lógica. Si tiene valor, la entidad está eliminada
    /// y no aparecerá en ningún listado.
    /// </summary>
    public DateTime? DeleteAt { get; set; }
}
