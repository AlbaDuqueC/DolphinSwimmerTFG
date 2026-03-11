using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Repositorio de acceso a datos para Rutina.
/// </summary>
public class RutinaRepository : IRutinaRepository
{
    private readonly AppDbContext _context;

    public RutinaRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Obtiene una rutina por su ID.</summary>
    public async Task<Rutina?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Rutinas
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Id == id);
        return resultado;
    }

    /// <summary>Obtiene todas las rutinas de un usuario.</summary>
    public async Task<IEnumerable<Rutina>> ObtenerPorUsuarioAsync(int idUsuario)
    {
        var resultado = await _context.Rutinas
            .Where(r => r.IdUsuario == idUsuario)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();
        return resultado;
    }

    /// <summary>Crea una nueva rutina.</summary>
    public async Task<Rutina> CrearAsync(Rutina rutina)
    {
        rutina.AplicarSlugCreacion();
        _context.Rutinas.Add(rutina);
        await _context.SaveChangesAsync();
        var resultado = rutina;
        return resultado;
    }

    /// <summary>Actualiza una rutina existente.</summary>
    public async Task<Rutina> ActualizarAsync(Rutina rutina)
    {
        rutina.AplicarSlugActualizacion();
        _context.Rutinas.Update(rutina);
        await _context.SaveChangesAsync();
        var resultado = rutina;
        return resultado;
    }

    /// <summary>Aplica eliminación lógica a una rutina.</summary>
    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var rutina = await _context.Rutinas.FindAsync(id);
        var resultado = false;

        if (rutina != null)
        {
            rutina.AplicarSlugEliminacion();
            await _context.SaveChangesAsync();
            resultado = true;
        }

        return resultado;
    }
}
