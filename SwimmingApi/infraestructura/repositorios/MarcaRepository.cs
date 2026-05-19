using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Implementación del repositorio de acceso a datos para MarcaDeTiempo.
/// Encapsula las operaciones de lectura y escritura contra la base de datos
/// usando Entity Framework.
/// </summary>
public class MarcaRepository : IMarcaRepository
{
    // Contexto de Entity Framework que da acceso a la base de datos.
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor con inyección de dependencias del contexto de base de datos.
    /// </summary>
    public MarcaRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene una marca de tiempo por su ID incluyendo sus relaciones
    /// (NadadorEquipo y Nadador) con eager loading.
    /// </summary>
    public async Task<MarcaDeTiempo?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.MarcasDeTiempo
            .Include(m => m.NadadorEquipo)
            .Include(m => m.Nadador)
            .FirstOrDefaultAsync(m => m.Id == id);
        return resultado;
    }

    /// <summary>
    /// Obtiene todas las marcas de tiempo asociadas a un NadadorEquipo concreto,
    /// ordenadas de la más reciente a la más antigua.
    /// </summary>
    public async Task<IEnumerable<MarcaDeTiempo>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo)
    {
        var resultado = await _context.MarcasDeTiempo
            .Where(m => m.IdNadadorEquipo == idNadadorEquipo)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>
    /// Obtiene todas las marcas de tiempo registradas por un nadador concreto,
    /// ordenadas de la más reciente a la más antigua.
    /// </summary>
    public async Task<IEnumerable<MarcaDeTiempo>> ObtenerPorNadadorAsync(int idNadador)
    {
        var resultado = await _context.MarcasDeTiempo
            .Where(m => m.IdNadador == idNadador)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>
    /// Registra una nueva marca de tiempo en la base de datos.
    /// Aplica automáticamente la fecha de creación antes de guardarla.
    /// </summary>
    public async Task<MarcaDeTiempo> CrearAsync(MarcaDeTiempo marca)
    {
        marca.AplicarSlugCreacion();
        _context.MarcasDeTiempo.Add(marca);
        await _context.SaveChangesAsync();
        var resultado = marca;
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de una marca de tiempo existente.
    /// Aplica automáticamente la fecha de modificación antes de guardar.
    /// </summary>
    public async Task<MarcaDeTiempo> ActualizarAsync(MarcaDeTiempo marca)
    {
        marca.AplicarSlugActualizacion();
        _context.MarcasDeTiempo.Update(marca);
        await _context.SaveChangesAsync();
        var resultado = marca;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica a una marca de tiempo.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var marca = await _context.MarcasDeTiempo.FindAsync(id);
        var resultado = false;

        if (marca != null)
        {
            marca.AplicarSlugEliminacion();
            await _context.SaveChangesAsync();
            resultado = true;
        }

        return resultado;
    }
}