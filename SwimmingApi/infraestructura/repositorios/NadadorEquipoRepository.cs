using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Implementación del repositorio de acceso a datos para NadadorEquipo.
/// Encapsula las operaciones de lectura y escritura contra la base de datos
/// usando Entity Framework.
/// </summary>
public class NadadorEquipoRepository : INadadorEquipoRepository
{
    // Contexto de Entity Framework que da acceso a la base de datos.
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor con inyección de dependencias del contexto de base de datos.
    /// </summary>
    public NadadorEquipoRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un NadadorEquipo por su ID incluyendo el equipo al que pertenece
    /// y la lista de marcas de tiempo asociadas.
    /// </summary>
    public async Task<NadadorEquipo?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.NadadoresEquipo
            .Include(ne => ne.Equipo)
            .Include(ne => ne.ListaDeTiempoEquipo)
            .FirstOrDefaultAsync(ne => ne.Id == id);
        return resultado;
    }

    /// <summary>
    /// Obtiene un NadadorEquipo a partir de su código único de 6 dígitos.
    /// Es la consulta clave para que un nadador pueda vincularse a su ficha del equipo.
    /// </summary>
    public async Task<NadadorEquipo?> ObtenerPorCodigoAsync(int codigo)
    {
        var resultado = await _context.NadadoresEquipo
            .Include(ne => ne.Equipo)
            .Include(ne => ne.ListaDeTiempoEquipo)
            .FirstOrDefaultAsync(ne => ne.Codigo == codigo);
        return resultado;
    }

    /// <summary>
    /// Obtiene todos los nadadores registrados en un equipo concreto,
    /// ordenados del más reciente al más antiguo.
    /// </summary>
    public async Task<IEnumerable<NadadorEquipo>> ObtenerPorEquipoAsync(int idEquipo)
    {
        var resultado = await _context.NadadoresEquipo
            .Where(ne => ne.IdEquipo == idEquipo)
            .Include(ne => ne.ListaDeTiempoEquipo)
            .OrderByDescending(ne => ne.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>
    /// Crea un nuevo NadadorEquipo en la base de datos.
    /// Aplica automáticamente la fecha de creación antes de guardarlo.
    /// </summary>
    public async Task<NadadorEquipo> CrearAsync(NadadorEquipo nadadorEquipo)
    {
        nadadorEquipo.AplicarSlugCreacion();
        _context.NadadoresEquipo.Add(nadadorEquipo);
        await _context.SaveChangesAsync();
        var resultado = nadadorEquipo;
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de un NadadorEquipo existente.
    /// Aplica automáticamente la fecha de modificación antes de guardar.
    /// </summary>
    public async Task<NadadorEquipo> ActualizarAsync(NadadorEquipo nadadorEquipo)
    {
        nadadorEquipo.AplicarSlugActualizacion();
        _context.NadadoresEquipo.Update(nadadorEquipo);
        await _context.SaveChangesAsync();
        var resultado = nadadorEquipo;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica a un NadadorEquipo.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
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