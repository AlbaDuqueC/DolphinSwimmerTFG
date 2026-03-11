using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos de Nadador.
/// </summary>
public interface INadadorRepository
{
    Task<Nadador?> ObtenerPorIdAsync(int id);
    Task<Nadador?> ObtenerPorEmailAsync(string email);
    Task<IEnumerable<Nadador>> ObtenerTodosAsync();
    Task<Nadador> CrearAsync(Nadador nadador);
    Task<Nadador> ActualizarAsync(Nadador nadador);
    Task<bool> EliminarLogicoAsync(int id);
    Task<bool> ExisteEmailAsync(string email);
}
