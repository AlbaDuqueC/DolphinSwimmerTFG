using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos de Entrenador.
/// </summary>
public interface IEntrenadorRepository
{
    Task<Entrenador?> ObtenerPorIdAsync(int id);
    Task<Entrenador?> ObtenerPorEmailAsync(string email);
    Task<IEnumerable<Entrenador>> ObtenerTodosAsync();
    Task<Entrenador> CrearAsync(Entrenador entrenador);
    Task<Entrenador> ActualizarAsync(Entrenador entrenador);
    Task<bool> EliminarLogicoAsync(int id);
    Task<bool> ExisteEmailAsync(string email);
}
