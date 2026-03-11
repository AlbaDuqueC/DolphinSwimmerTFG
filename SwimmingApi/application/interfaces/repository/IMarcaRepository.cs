using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos de MarcaDeTiempo.
/// </summary>
public interface IMarcaRepository
{
    Task<MarcaDeTiempo?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<MarcaDeTiempo>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo);
    Task<MarcaDeTiempo> CrearAsync(MarcaDeTiempo marca);
    Task<MarcaDeTiempo> ActualizarAsync(MarcaDeTiempo marca);
    Task<bool> EliminarLogicoAsync(int id);
}
