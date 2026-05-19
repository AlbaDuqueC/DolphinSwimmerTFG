using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace SwimmingApi.Infraestructura.Rider;

/// <summary>
/// Servicio de caché en memoria para evitar consultas repetidas a la base de datos.
/// Guarda en RAM los datos más solicitados durante un tiempo limitado,
/// reduciendo la carga del servidor y mejorando los tiempos de respuesta.
/// </summary>
public class CacheService
{
    // Implementación de caché en memoria proporcionada por .NET.
    private readonly IMemoryCache _cache;

    // Tiempo de vida por defecto de cada entrada de caché: 10 minutos.
    // Pasado ese tiempo, el dato se elimina automáticamente y la próxima consulta
    // tendrá que ir a la base de datos.
    private static readonly TimeSpan _tiempoExpiracion = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Constructor con inyección de dependencias del servicio de caché de .NET.
    /// </summary>
    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// Obtiene un valor del caché a partir de su clave.
    /// Devuelve el valor por defecto del tipo (null para objetos) si no existe.
    /// </summary>
    public T? Obtener<T>(string clave)
    {
        var resultado = _cache.TryGetValue(clave, out T? valor) ? valor : default;
        return resultado;
    }

    /// <summary>
    /// Guarda un valor en el caché con el tiempo de expiración por defecto.
    /// Si la clave ya existía, se sobrescribe.
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
    /// Elimina manualmente un valor del caché.
    /// Se usa al modificar o borrar registros para que la próxima consulta
    /// devuelva datos frescos desde la base de datos.
    /// </summary>
    public void Eliminar(string clave)
    {
        _cache.Remove(clave);
    }

    /// <summary>
    /// Genera una clave de caché estándar para una entidad concreta.
    /// Por ejemplo: GenerarClave("nadador", 5) devuelve "nadador:5".
    /// </summary>
    public string GenerarClave(string entidad, int id)
    {
        var resultado = $"{entidad}:{id}";
        return resultado;
    }

    /// <summary>
    /// Genera una clave de caché para listas completas de una entidad.
    /// Por ejemplo: GenerarClaveLista("equipo") devuelve "equipo:lista".
    /// </summary>
    public string GenerarClaveLista(string entidad)
    {
        var resultado = $"{entidad}:lista";
        return resultado;
    }
}