using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Repositorio de acceso a datos para Equipo.
/// </summary>
public class EquipoRepository : IEquipoRepository
{
    private readonly AppDbContext _context;

    public EquipoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Obtiene un equipo por su ID con la lista de nadadores.</summary>
    public async Task<Equipo?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Equipos
            .Include(e => e.ListaNadadores)
            .FirstOrDefaultAsync(e => e.Id == id);
        return resultado;
    }

    /// <summary>Obtiene todos los equipos activos.</summary>
    public async Task<IEnumerable<Equipo>> ObtenerTodosAsync()
    {
        var resultado = await _context.Equipos
            .Include(e => e.ListaNadadores)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>Crea un nuevo equipo.</summary>
    public async Task<Equipo> CrearAsync(Equipo equipo)
    {
        equipo.AplicarSlugCreacion();
        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();
        var resultado = equipo;
        return resultado;
    }

    /// <summary>Actualiza un equipo existente.</summary>
    public async Task<Equipo> ActualizarAsync(Equipo equipo)
    {
        equipo.AplicarSlugActualizacion();
        _context.Equipos.Update(equipo);
        await _context.SaveChangesAsync();
        var resultado = equipo;
        return resultado;
    }

    /// <summary>Aplica eliminación lógica a un equipo.</summary>
    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var equipo = await _context.Equipos.FindAsync(id);
        var resultado = false;

        if (equipo != null)
        {
            equipo.AplicarSlugEliminacion();
            await _context.SaveChangesAsync();
            resultado = true;
        }

        return resultado;
    }
}
