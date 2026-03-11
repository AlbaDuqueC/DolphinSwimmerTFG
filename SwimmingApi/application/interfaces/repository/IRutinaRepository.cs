using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos de Rutina.
/// </summary>
public interface IRutinaRepository
{
    Task<Rutina?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<Rutina>> ObtenerPorUsuarioAsync(int idUsuario);
    Task<Rutina> CrearAsync(Rutina rutina);
    Task<Rutina> ActualizarAsync(Rutina rutina);
    Task<bool> EliminarLogicoAsync(int id);
}
