using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Infraestructura.Extensions;

/// <summary>
/// Extensiones para consultas comunes sobre IQueryable.
/// </summary>
public static class QueryExtensions
{
    /// <summary>
    /// Ordena una lista de entidades por fecha de creación de más reciente a más antigua.
    /// </summary>
    public static IQueryable<T> OrderByReciente<T>(this IQueryable<T> query)
        where T : EntityBase
    {
        var resultado = query.OrderByDescending(e => e.CreatedAt);
        return resultado;
    }

    /// <summary>
    /// Ordena una lista de entidades por fecha de creación de más antigua a más reciente.
    /// </summary>
    public static IQueryable<T> OrderByAntiguo<T>(this IQueryable<T> query)
        where T : EntityBase
    {
        var resultado = query.OrderBy(e => e.CreatedAt);
        return resultado;
    }

    /// <summary>
    /// Filtra solo las entidades que no han sido eliminadas lógicamente.
    /// Útil cuando se desactiva el filtro global.
    /// </summary>
    public static IQueryable<T> SoloActivos<T>(this IQueryable<T> query)
        where T : EntityBase
    {
        var resultado = query.Where(e => e.DeleteAt == null);
        return resultado;
    }
}
