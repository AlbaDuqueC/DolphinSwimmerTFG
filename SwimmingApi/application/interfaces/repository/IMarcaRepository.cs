using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos relacionadas con MarcaDeTiempo.
/// Define las operaciones que cualquier implementación debe respetar.
/// </summary>
public interface IMarcaRepository
{
    /// <summary>Obtiene una marca de tiempo por su ID. Devuelve null si no existe.</summary>
    Task<MarcaDeTiempo?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene todas las marcas de tiempo asociadas a un NadadorEquipo concreto.</summary>
    Task<IEnumerable<MarcaDeTiempo>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo);

    /// <summary>Obtiene todas las marcas de tiempo registradas por un nadador.</summary>
    Task<IEnumerable<MarcaDeTiempo>> ObtenerPorNadadorAsync(int idNadador);

    /// <summary>Crea una nueva marca de tiempo en la base de datos.</summary>
    Task<MarcaDeTiempo> CrearAsync(MarcaDeTiempo marca);

    /// <summary>Actualiza los datos de una marca de tiempo existente.</summary>
    Task<MarcaDeTiempo> ActualizarAsync(MarcaDeTiempo marca);

    /// <summary>
    /// Elimina lógicamente una marca de tiempo (marca el registro como inactivo
    /// sin borrarlo de la base de datos).
    /// </summary>
    Task<bool> EliminarLogicoAsync(int id);
}