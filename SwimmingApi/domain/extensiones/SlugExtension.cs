using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Extensiones;

/// <summary>
/// Extensiones para asignar valores por defecto a las entidades (Slug).
/// Se aplican automáticamente antes de crear o modificar una entidad.
/// </summary>
public static class SlugExtension
{
    /// <summary>
    /// Asigna la fecha de creación al momento actual si no está definida.
    /// </summary>
    public static EntityBase AplicarSlugCreacion(this Domain.Entities.EntityBase entidad)
    {
        var resultado = entidad;
        resultado.CreatedAt = DateTime.UtcNow;
        resultado.UpdateAt = null;
        resultado.DeleteAt = null;
        return resultado;
    }

    /// <summary>
    /// Actualiza la fecha de modificación al momento actual.
    /// </summary>
    public static Domain.Entities.EntityBase AplicarSlugActualizacion(this Domain.Entities.EntityBase entidad)
    {
        var resultado = entidad;
        resultado.UpdateAt = DateTime.UtcNow;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica insertando la fecha de eliminación.
    /// La entidad no se borra de la base de datos.
    /// </summary>
    public static Domain.Entities.EntityBase AplicarSlugEliminacion(this Domain.Entities.EntityBase entidad)
    {
        var resultado = entidad;
        resultado.DeleteAt = DateTime.UtcNow;
        return resultado;
    }
}
