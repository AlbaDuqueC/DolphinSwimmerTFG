using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Repositorio de acceso a datos para Entrenador.
/// </summary>
public class EntrenadorRepository : IEntrenadorRepository
{
    private readonly AppDbContext _context;

    public EntrenadorRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Obtiene un entrenador por su ID con sus relaciones.</summary>
    public async Task<Entrenador?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Entrenadores
            .Include(e => e.EquipoGestionado)
            .Include(e => e.ListaRutinas)
            .FirstOrDefaultAsync(e => e.Id == id);
        return resultado;
    }

    /// <summary>Obtiene un entrenador por su email.</summary>
    public async Task<Entrenador?> ObtenerPorEmailAsync(string email)
    {
        var resultado = await _context.Entrenadores
            .FirstOrDefaultAsync(e => e.Email == email);
        return resultado;
    }

    /// <summary>Obtiene todos los entrenadores activos.</summary>
    public async Task<IEnumerable<Entrenador>> ObtenerTodosAsync()
    {
        var resultado = await _context.Entrenadores
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>Crea un nuevo entrenador.</summary>
    public async Task<Entrenador> CrearAsync(Entrenador entrenador)
    {
        entrenador.AplicarSlugCreacion();
        _context.Entrenadores.Add(entrenador);
        await _context.SaveChangesAsync();
        var resultado = entrenador;
        return resultado;
    }

    /// <summary>Actualiza un entrenador existente.</summary>
    public async Task<Entrenador> ActualizarAsync(Entrenador entrenador)
    {
        entrenador.AplicarSlugActualizacion();
        _context.Entrenadores.Update(entrenador);
        await _context.SaveChangesAsync();
        var resultado = entrenador;
        return resultado;
    }

    /// <summary>Aplica eliminación lógica a un entrenador.</summary>
    public async Task<bool> EliminarLogicoAsync(int id)
    {
        var entrenador = await _context.Entrenadores.FindAsync(id);
        var resultado = false;

        if (entrenador != null)
        {
            entrenador.AplicarSlugEliminacion();
            await _context.SaveChangesAsync();
            resultado = true;
        }

        return resultado;
    }

    /// <summary>Comprueba si ya existe un entrenador con ese email.</summary>
    public async Task<bool> ExisteEmailAsync(string email)
    {
        var resultado = await _context.Entrenadores.AnyAsync(e => e.Email == email);
        return resultado;
    }
}
