using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos relacionadas con Entrenador.
/// Define el conjunto de operaciones que cualquier implementación debe respetar,
/// permitiendo desacoplar la lógica de negocio de la tecnología concreta (Entity Framework, etc.).
/// </summary>
public interface IEntrenadorRepository
{
    /// <summary>Obtiene un entrenador por su ID. Devuelve null si no existe.</summary>
    Task<Entrenador?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene un entrenador por su correo electrónico. Devuelve null si no existe.</summary>
    Task<Entrenador?> ObtenerPorEmailAsync(string email);

    /// <summary>Obtiene la lista de todos los entrenadores activos.</summary>
    Task<IEnumerable<Entrenador>> ObtenerTodosAsync();

    /// <summary>Crea un nuevo entrenador en la base de datos.</summary>
    Task<Entrenador> CrearAsync(Entrenador entrenador);

    /// <summary>Actualiza los datos de un entrenador existente.</summary>
    Task<Entrenador> ActualizarAsync(Entrenador entrenador);

    /// <summary>
    /// Elimina lógicamente un entrenador (marca el registro como inactivo
    /// sin borrarlo de la base de datos).
    /// </summary>
    Task<bool> EliminarLogicoAsync(int id);

    /// <summary>Comprueba si existe ya un entrenador con el email indicado.</summary>
    Task<bool> ExisteEmailAsync(string email);
}