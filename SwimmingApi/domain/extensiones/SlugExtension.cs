using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Extensiones;

/// <summary>
/// Clase de métodos de extensión que asignan automáticamente los campos
/// de auditoría (CreatedAt, UpdateAt, DeleteAt) a las entidades.
/// Centraliza la lógica de fechas para que cada repositorio no tenga que repetirla.
/// </summary>
public static class SlugExtension
{
    /// <summary>
    /// Inicializa los campos de auditoría al crear una entidad nueva.
    /// Asigna la fecha de creación al momento actual (UTC) y deja
    /// los campos UpdateAt y DeleteAt vacíos.
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
    /// Actualiza el campo UpdateAt al momento actual (UTC).
    /// Se llama cada vez que se modifica una entidad existente.
    /// </summary>
    public static Domain.Entities.EntityBase AplicarSlugActualizacion(this Domain.Entities.EntityBase entidad)
    {
        var resultado = entidad;
        resultado.UpdateAt = DateTime.UtcNow;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica asignando la fecha de eliminación.
    /// La entidad sigue existiendo físicamente en la base de datos
    /// pero las consultas la filtran como si estuviera borrada.
    /// </summary>
    public static Domain.Entities.EntityBase AplicarSlugEliminacion(this Domain.Entities.EntityBase entidad)
    {
        var resultado = entidad;
        resultado.DeleteAt = DateTime.UtcNow;
        return resultado;
    }
}