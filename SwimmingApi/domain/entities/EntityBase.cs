namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad base abstracta de la que heredan todas las entidades del sistema.
/// Define los campos comunes para auditoría (CreatedAt, UpdateAt)
/// y para eliminación lógica (DeleteAt), evitando duplicarlos en cada entidad.
/// </summary>
public abstract class EntityBase
{
    /// <summary>Identificador único de la entidad en la base de datos.</summary>
    public int Id { get; set; }

    /// <summary>Fecha y hora en la que se creó la entidad. Se asigna automáticamente al crear el registro.</summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Fecha y hora de la última modificación de la entidad.
    /// Es nula mientras la entidad no se haya modificado nunca.
    /// </summary>
    public DateTime? UpdateAt { get; set; }

    /// <summary>
    /// Fecha y hora de eliminación lógica.
    /// Si tiene valor, la entidad se considera eliminada y no aparecerá en ningún listado,
    /// aunque siga existiendo físicamente en la base de datos.
    /// </summary>
    public DateTime? DeleteAt { get; set; }
}