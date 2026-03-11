using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Repositorio de acceso a datos para NadadorEquipo.
/// </summary>
public class NadadorEquipoRepository : INadadorEquipoRepository
{
    private readonly AppDbContext _context;

    public NadadorEquipoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Obtiene un NadadorEquipo por su ID.</summary>
    public async Task<NadadorEquipo?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.NadadoresEquipo
            .Include(ne => ne.Equipo)
            .Include(ne => ne.ListaDeTiempoEquipo)
            .FirstOrDefaultAsync(ne => ne.Id == id);
        return resultado;
    }

    /// <summary>Obtiene un NadadorEquipo por su código único.</summary>
    public async Task<NadadorEquipo?> ObtenerPorCodigoAsync(int codigo)
    {
        var resultado = await _context.NadadoresEquipo
            .Include(ne => ne.Equipo)
            .Include(ne => ne.ListaDeTiempoEquipo)
            .FirstOrDefaultAsync(ne => ne.Codigo == codigo);
        return resultado;
    }

    /// <summary>Obtiene todos los nadadores de un equipo específico.</summary>
    public async Task<IEnumerable<NadadorEquipo>> ObtenerPorEquipoAsync(int idEquipo)
    {
        var resultado = await _context.NadadoresEquipo
            .Where(ne => ne.IdEquipo == idEquipo)
            .Include(ne => ne.ListaDeTiempoEquipo)
            .OrderByDescending(ne => ne.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>Crea un nuevo NadadorEquipo.</summary>
    public async Task<NadadorEquipo> CrearAsync(NadadorEquipo nadadorEquipo)
    {
        nadadorEquipo.AplicarSlugCreacion();
        _context.NadadoresEquipo.Add(nadadorEquipo);
        await _context.SaveChangesAsync();
        var resultado = nadadorEquipo;
        return resultado;
    }

    /// <summary>Actualiza un NadadorEquipo existente.</summary>
    public async Task<NadadorEquipo> ActualizarAsync(NadadorEquipo nadadorEquipo)
    {
        nadadorEquipo.AplicarSlugActualizacion();
        _context.NadadoresEquipo.Update(nadadorEquipo);
        await _context.SaveChangesAsync();
        var resultado = nadadorEquipo;
        return resultado;
    }

    /// <summary>Aplica eliminación lógica a un NadadorEquipo.</summary>
    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var nadadorEquipo = await _context.NadadoresEquipo.FindAsync(id);
        var resultado = false;

        if (nadadorEquipo != null)
        {
            nadadorEquipo.AplicarSlugEliminacion();
            await _context.SaveChangesAsync();
            resultado = true;
        }

        return resultado;
    }
}
