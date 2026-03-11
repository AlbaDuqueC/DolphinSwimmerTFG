using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos de NadadorEquipo.
/// </summary>
public interface INadadorEquipoRepository
{
    Task<NadadorEquipo?> ObtenerPorIdAsync(int id);
    Task<NadadorEquipo?> ObtenerPorCodigoAsync(int codigo);
    Task<IEnumerable<NadadorEquipo>> ObtenerPorEquipoAsync(int idEquipo);
    Task<NadadorEquipo> CrearAsync(NadadorEquipo nadadorEquipo);
    Task<NadadorEquipo> ActualizarAsync(NadadorEquipo nadadorEquipo);
    Task<bool> EliminarLogicoAsync(int id);
}
