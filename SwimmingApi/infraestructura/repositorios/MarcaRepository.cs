using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Repositorio de acceso a datos para MarcaDeTiempo.
/// </summary>
public class MarcaRepository : IMarcaRepository
{
    private readonly AppDbContext _context;

    public MarcaRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Obtiene una marca de tiempo por su ID.</summary>
    public async Task<MarcaDeTiempo?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.MarcasDeTiempo
            .Include(m => m.NadadorEquipo)
            .Include(m => m.Nadador)
            .FirstOrDefaultAsync(m => m.Id == id);
        return resultado;
    }

    /// <summary>Obtiene todas las marcas de tiempo de un NadadorEquipo.</summary>
    public async Task<IEnumerable<MarcaDeTiempo>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo)
    {
        var resultado = await _context.MarcasDeTiempo
            .Where(m => m.IdNadadorEquipo == idNadadorEquipo)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>Obtiene todas las marcas de tiempo registradas por un nadador.</summary>
    public async Task<IEnumerable<MarcaDeTiempo>> ObtenerPorNadadorAsync(int idNadador)
    {
        var resultado = await _context.MarcasDeTiempo
            .Where(m => m.IdNadador == idNadador)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>Registra una nueva marca de tiempo.</summary>
    public async Task<MarcaDeTiempo> CrearAsync(MarcaDeTiempo marca)
    {
        marca.AplicarSlugCreacion();
        _context.MarcasDeTiempo.Add(marca);
        await _context.SaveChangesAsync();
        var resultado = marca;
        return resultado;
    }

    /// <summary>Actualiza una marca de tiempo existente.</summary>
    public async Task<MarcaDeTiempo> ActualizarAsync(MarcaDeTiempo marca)
    {
        marca.AplicarSlugActualizacion();
        _context.MarcasDeTiempo.Update(marca);
        await _context.SaveChangesAsync();
        var resultado = marca;
        return resultado;
    }

    /// <summary>Aplica eliminación lógica a una marca de tiempo.</summary>
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
