using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Implementación del repositorio de acceso a datos para Equipo.
/// Encapsula las operaciones de lectura y escritura contra la base de datos
/// usando Entity Framework.
/// </summary>
public class EquipoRepository : IEquipoRepository
{
    // Contexto de Entity Framework que da acceso a la base de datos.
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor con inyección de dependencias del contexto de base de datos.
    /// </summary>
    public EquipoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un equipo por su ID incluyendo la lista de nadadores que pertenecen a él.
    /// </summary>
    public async Task<Equipo?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Equipos
            .Include(e => e.ListaNadadores)
            .FirstOrDefaultAsync(e => e.Id == id);
        return resultado;
    }

    /// <summary>
    /// Obtiene todos los equipos activos ordenados por fecha de creación
    /// del más reciente al más antiguo, incluyendo sus nadadores.
    /// </summary>
    public async Task<IEnumerable<Equipo>> ObtenerTodosAsync()
    {
        var resultado = await _context.Equipos
            .Include(e => e.ListaNadadores)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>
    /// Crea un nuevo equipo en la base de datos.
    /// Aplica automáticamente la fecha de creación antes de guardarlo.
    /// </summary>
    public async Task<Equipo> CrearAsync(Equipo equipo)
    {
        equipo.AplicarSlugCreacion();
        _context.Equipos.Add(equipo);
        await _context.SaveChangesAsync();
        var resultado = equipo;
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de un equipo existente.
    /// Aplica automáticamente la fecha de modificación antes de guardar.
    /// </summary>
    public async Task<Equipo> ActualizarAsync(Equipo equipo)
    {
        equipo.AplicarSlugActualizacion();
        _context.Equipos.Update(equipo);
        await _context.SaveChangesAsync();
        var resultado = equipo;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica a un equipo.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
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