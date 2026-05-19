using Microsoft.EntityFrameworkCore;
using SwimmingApi.Application.Interfaces.Repository;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Extensiones;
using SwimmingApi.Infraestructura.Database;

namespace SwimmingApi.Infraestructura.Repositorios;

/// <summary>
/// Implementación del repositorio de acceso a datos para Nadador.
/// Encapsula las operaciones de lectura y escritura contra la base de datos
/// usando Entity Framework, y aplica automáticamente los timestamps de auditoría.
/// </summary>
public class NadadorRepository : INadadorRepository
{
    // Contexto de Entity Framework que da acceso a la base de datos.
    private readonly AppDbContext _context;

    /// <summary>
    /// Constructor con inyección de dependencias del contexto de base de datos.
    /// </summary>
    public NadadorRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Obtiene un nadador por su ID incluyendo sus relaciones
    /// (NadadorEquipo, rutinas y marcas de tiempo) con eager loading.
    /// </summary>
    public async Task<Nadador?> ObtenerPorIdAsync(int id)
    {
        var resultado = await _context.Nadadores
            .Include(n => n.NadadorEquipo)
            .Include(n => n.ListaRutinas)
            .Include(n => n.ListaDeTiempo)
            .FirstOrDefaultAsync(n => n.Id == id);
        return resultado;
    }

    /// <summary>
    /// Obtiene un nadador por su email.
    /// Devuelve null si no existe ninguno con ese email.
    /// </summary>
    public async Task<Nadador?> ObtenerPorEmailAsync(string email)
    {
        var resultado = await _context.Nadadores
            .FirstOrDefaultAsync(n => n.Email == email);
        return resultado;
    }

    /// <summary>
    /// Obtiene todos los nadadores activos ordenados por fecha de creación
    /// del más reciente al más antiguo.
    /// </summary>
    public async Task<IEnumerable<Nadador>> ObtenerTodosAsync()
    {
        var resultado = await _context.Nadadores
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
        return resultado;
    }

    /// <summary>
    /// Crea un nuevo nadador en la base de datos.
    /// Aplica automáticamente la fecha de creación antes de guardarlo.
    /// </summary>
    public async Task<Nadador> CrearAsync(Nadador nadador)
    {
        nadador.AplicarSlugCreacion();
        _context.Nadadores.Add(nadador);
        await _context.SaveChangesAsync();
        var resultado = nadador;
        return resultado;
    }

    /// <summary>
    /// Actualiza los datos de un nadador existente.
    /// Aplica automáticamente la fecha de modificación antes de guardar.
    /// </summary>
    public async Task<Nadador> ActualizarAsync(Nadador nadador)
    {
        nadador.AplicarSlugActualizacion();
        _context.Nadadores.Update(nadador);
        await _context.SaveChangesAsync();
        var resultado = nadador;
        return resultado;
    }

    /// <summary>
    /// Aplica la eliminación lógica a un nadador.
    /// El registro permanece en la base de datos pero se marca como inactivo.
    /// </summary>
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

    /// <summary>
    /// Comprueba si ya existe un nadador registrado con el email indicado.
    /// Útil al crear cuentas para evitar duplicados.
    /// </summary>
    public async Task<bool> ExisteEmailAsync(string email)
    {
        var resultado = await _context.Nadadores.AnyAsync(n => n.Email == email);
        return resultado;
    }
}