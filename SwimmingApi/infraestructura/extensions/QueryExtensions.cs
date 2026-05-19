using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Infraestructura.Extensions;

/// <summary>
/// Métodos de extensión para consultas LINQ comunes sobre cualquier IQueryable
/// que herede de EntityBase. Permiten reutilizar lógica de ordenación
/// y filtrado sin tener que repetirla en cada repositorio.
/// </summary>
public static class QueryExtensions
{
    /// <summary>
    /// Ordena las entidades por fecha de creación, mostrando primero las más recientes.
    /// </summary>
    public static IQueryable<T> OrderByReciente<T>(this IQueryable<T> query)
        where T : EntityBase
    {
        var resultado = query.OrderByDescending(e => e.CreatedAt);
        return resultado;
    }

    /// <summary>
    /// Ordena las entidades por fecha de creación, mostrando primero las más antiguas.
    /// </summary>
    public static IQueryable<T> OrderByAntiguo<T>(this IQueryable<T> query)
        where T : EntityBase
    {
        var resultado = query.OrderBy(e => e.CreatedAt);
        return resultado;
    }

    /// <summary>
    /// Filtra solo las entidades que no han sido eliminadas lógicamente.
    /// Útil cuando se ha desactivado el filtro global del DbContext con IgnoreQueryFilters.
    /// </summary>
    public static IQueryable<T> SoloActivos<T>(this IQueryable<T> query)
        where T : EntityBase
    {
        var resultado = query.Where(e => e.DeleteAt == null);
        return resultado;
    }
}