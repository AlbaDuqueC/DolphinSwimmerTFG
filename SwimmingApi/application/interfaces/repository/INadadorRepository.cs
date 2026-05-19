using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos relacionadas con Nadador.
/// Define las operaciones que cualquier implementación debe respetar.
/// </summary>
public interface INadadorRepository
{
    /// <summary>Obtiene un nadador por su ID. Devuelve null si no existe.</summary>
    Task<Nadador?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene un nadador por su correo electrónico. Devuelve null si no existe.</summary>
    Task<Nadador?> ObtenerPorEmailAsync(string email);

    /// <summary>Obtiene la lista de todos los nadadores activos.</summary>
    Task<IEnumerable<Nadador>> ObtenerTodosAsync();

    /// <summary>Crea un nuevo nadador en la base de datos.</summary>
    Task<Nadador> CrearAsync(Nadador nadador);

    /// <summary>Actualiza los datos de un nadador existente.</summary>
    Task<Nadador> ActualizarAsync(Nadador nadador);

    /// <summary>
    /// Elimina lógicamente un nadador (marca el registro como inactivo
    /// sin borrarlo de la base de datos).
    /// </summary>
    Task<bool> EliminarLogicoAsync(int id);

    /// <summary>Comprueba si existe ya un nadador con el email indicado.</summary>
    Task<bool> ExisteEmailAsync(string email);
}