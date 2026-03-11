using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos de Equipo.
/// </summary>
public interface IEquipoRepository
{
    Task<Equipo?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<Equipo>> ObtenerTodosAsync();
    Task<Equipo> CrearAsync(Equipo equipo);
    Task<Equipo> ActualizarAsync(Equipo equipo);
    Task<bool> EliminarLogicoAsync(int id);
}
