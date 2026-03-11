using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace SwimmingApi.Infraestructura.Rider;

/// <summary>
/// Servicio de caché en memoria para evitar peticiones repetidas a la base de datos.
/// Actúa como la capa "Rider" del sistema.
/// </summary>
public class CacheService
{
    private readonly IMemoryCache _cache;
    private static readonly TimeSpan _tiempoExpiracion = TimeSpan.FromMinutes(10);

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Obtiene un valor del caché. Devuelve null si no existe.
    /// </summary>
    public T? Obtener<T>(string clave)
    {
        var resultado = _cache.TryGetValue(clave, out T? valor) ? valor : default;
        return resultado;
    }

    /// <summary>
    /// Guarda un valor en el caché con tiempo de expiración por defecto.
    /// </summary>
    public void Guardar<T>(string clave, T valor)
    {
        var opciones = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = _tiempoExpiracion
        };
        _cache.Set(clave, valor, opciones);
    }

    /// <summary>
    /// Elimina un valor del caché por su clave.
    /// </summary>
    public void Eliminar(string clave)
    {
        _cache.Remove(clave);
    }

    /// <summary>
    /// Genera una clave de caché estándar para una entidad.
    /// </summary>
    public string GenerarClave(string entidad, int id)
    {
        var resultado = $"{entidad}:{id}";
        return resultado;
    }

    /// <summary>
    /// Genera una clave de caché para listas.
    /// </summary>
    public string GenerarClaveLista(string entidad)
    {
        var resultado = $"{entidad}:lista";
        return resultado;
    }
}
