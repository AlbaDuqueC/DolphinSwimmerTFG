using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Implementación del repositorio de acceso a datos para Rutina.
/// Encapsula las operaciones de lectura y escritura contra la base de datos
/// usando Entity Framework.
/// </summary>
public class RutinaRepository : IRutinaRepository
{
    // Contexto de Entity Framework que da acceso a la base de datos.
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor con inyección de dependencias del contexto de base de datos.
    /// </summary>
    public RutinaRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene una rutina por su ID incluyendo el usuario propietario.
    /// </summary>
    public async Task<Rutina?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Rutinas
            .Include(r => r.Usuario)
            .FirstOrDefaultAsync(r => r.Id == id);
        return resultado;
    }

    /// <summary>
    /// Obtiene todas las rutinas de un usuario concreto,
    /// ordenadas por fecha de la más reciente a la más antigua.
    /// </summary>
    public async Task<IEnumerable<Rutina>> ObtenerPorUsuarioAsync(int idUsuario)
    {
        var resultado = await _context.Rutinas
            .Where(r => r.IdUsuario == idUsuario)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();
        return resultado;
    }

    /// <summary>
    /// Crea una nueva rutina en la base de datos.
    /// Aplica automáticamente la fecha de creación antes de guardarla.
    /// </summary>
    public async Task<Rutina> CrearAsync(Rutina rutina)
    {
        rutina.AplicarSlugCreacion();
        _context.Rutinas.Add(rutina);
        await _context.SaveChangesAsync();
        var resultado = rutina;
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de una rutina existente.
    /// Aplica automáticamente la fecha de modificación antes de guardar.
    /// </summary>
    public async Task<Rutina> ActualizarAsync(Rutina rutina)
    {
        rutina.AplicarSlugActualizacion();
        _context.Rutinas.Update(rutina);
        await _context.SaveChangesAsync();
        var resultado = rutina;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica a una rutina.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
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