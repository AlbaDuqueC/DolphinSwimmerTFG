using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Repositorio de acceso a datos para la entidad Nadador.
/// </summary>
public class NadadorRepository : INadadorRepository
{
    private readonly AppDbContext _context;

    public NadadorRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Obtiene un nadador por su ID incluyendo sus relaciones.</summary>
    public async Task<Nadador?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Nadadores
            .Include(n => n.NadadorEquipo)
            .Include(n => n.ListaRutinas)
            .Include(n => n.ListaDeTiempo)
            .FirstOrDefaultAsync(n => n.Id == id);
        return resultado;
    }

    /// <summary>Obtiene un nadador por su email.</summary>
    public async Task<Nadador?> ObtenerPorEmailAsync(string email)
    {
        var resultado = await _context.Nadadores
            .FirstOrDefaultAsync(n => n.Email == email);
        return resultado;
    }

    /// <summary>Obtiene todos los nadadores activos.</summary>
    public async Task<IEnumerable<Nadador>> ObtenerTodosAsync()
    {
        var resultado = await _context.Nadadores
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>Crea un nuevo nadador en la base de datos.</summary>
    public async Task<Nadador> CrearAsync(Nadador nadador)
    {
        nadador.AplicarSlugCreacion();
        _context.Nadadores.Add(nadador);
        await _context.SaveChangesAsync();
        var resultado = nadador;
        return resultado;
    }

    /// <summary>Actualiza los datos de un nadador existente.</summary>
    public async Task<Nadador> ActualizarAsync(Nadador nadador)
    {
        nadador.AplicarSlugActualizacion();
        _context.Nadadores.Update(nadador);
        await _context.SaveChangesAsync();
        var resultado = nadador;
        return resultado;
    }

    /// <summary>Aplica eliminación lógica a un nadador.</summary>
    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var nadador = await _context.Nadadores.FindAsync(id);
        var resultado = false;

        if (nadador != null)
        {
            nadador.AplicarSlugEliminacion();
            await _context.SaveChangesAsync();
            resultado = true;
        }

        return resultado;
    }

    /// <summary>Comprueba si ya existe un nadador con ese email.</summary>
    public async Task<bool> ExisteEmailAsync(string email)
    {
        var resultado = await _context.Nadadores.AnyAsync(n => n.Email == email);
        return resultado;
    }
}
