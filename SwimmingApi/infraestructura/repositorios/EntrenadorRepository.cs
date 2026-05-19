using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Implementación del repositorio de acceso a datos para Entrenador.
/// Encapsula todas las operaciones de lectura y escritura contra la base de datos
/// usando Entity Framework, y aplica automáticamente los timestamps de auditoría.
/// </summary>
public class EntrenadorRepository : IEntrenadorRepository
{
    // Contexto de Entity Framework que da acceso a la base de datos.
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor con inyección de dependencias del contexto de base de datos.
    /// </summary>
    public EntrenadorRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un entrenador por su ID incluyendo sus relaciones
    /// (equipo gestionado y lista de rutinas) con eager loading.
    /// </summary>
    public async Task<Entrenador?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Entrenadores
            .Include(e => e.EquipoGestionado)
            .Include(e => e.ListaRutinas)
            .FirstOrDefaultAsync(e => e.Id == id);
        return resultado;
    }

    /// <summary>
    /// Obtiene un entrenador por su email.
    /// Devuelve null si no existe ninguno con ese email.
    /// </summary>
    public async Task<Entrenador?> ObtenerPorEmailAsync(string email)
    {
        var resultado = await _context.Entrenadores
            .FirstOrDefaultAsync(e => e.Email == email);
        return resultado;
    }

    /// <summary>
    /// Obtiene todos los entrenadores activos ordenados por fecha de creación
    /// del más reciente al más antiguo.
    /// </summary>
    public async Task<IEnumerable<Entrenador>> ObtenerTodosAsync()
    {
        var resultado = await _context.Entrenadores
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>
    /// Crea un nuevo entrenador en la base de datos.
    /// Aplica automáticamente la fecha de creación antes de guardarlo.
    /// </summary>
    public async Task<Entrenador> CrearAsync(Entrenador entrenador)
    {
        entrenador.AplicarSlugCreacion();
        _context.Entrenadores.Add(entrenador);
        await _context.SaveChangesAsync();
        var resultado = entrenador;
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de un entrenador existente.
    /// Aplica automáticamente la fecha de modificación antes de guardar.
    /// </summary>
    public async Task<Entrenador> ActualizarAsync(Entrenador entrenador)
    {
        entrenador.AplicarSlugActualizacion();
        _context.Entrenadores.Update(entrenador);
        await _context.SaveChangesAsync();
        var resultado = entrenador;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica a un entrenador.
    /// El registro permanece en la base de datos pero se marca como inactivo
    /// asignándole la fecha de eliminación, por lo que ya no aparecerá en consultas.
    /// </summary>
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

    /// <summary>
    /// Comprueba si ya existe un entrenador registrado con el email indicado.
    /// Útil al crear cuentas para evitar duplicados.
    /// </summary>
    public async Task<bool> ExisteEmailAsync(string email)
    {
        var resultado = await _context.Entrenadores.AnyAsync(e => e.Email == email);
        return resultado;
    }
}