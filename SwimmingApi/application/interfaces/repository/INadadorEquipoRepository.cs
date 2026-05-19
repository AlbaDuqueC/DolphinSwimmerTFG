using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos relacionadas con NadadorEquipo.
/// Define las operaciones que cualquier implementación debe respetar.
/// </summary>
public interface INadadorEquipoRepository
{
    /// <summary>Obtiene un NadadorEquipo por su ID. Devuelve null si no existe.</summary>
    Task<NadadorEquipo?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Obtiene un NadadorEquipo por su código único de 6 dígitos.
    /// Devuelve null si no se encuentra ninguno con ese código.
    /// </summary>
    Task<NadadorEquipo?> ObtenerPorCodigoAsync(int codigo);

    /// <summary>Obtiene todos los nadadores registrados en un equipo concreto.</summary>
    Task<IEnumerable<NadadorEquipo>> ObtenerPorEquipoAsync(int idEquipo);

    /// <summary>Crea un nuevo NadadorEquipo en la base de datos.</summary>
    Task<NadadorEquipo> CrearAsync(NadadorEquipo nadadorEquipo);

    /// <summary>Actualiza los datos de un NadadorEquipo existente.</summary>
    Task<NadadorEquipo> ActualizarAsync(NadadorEquipo nadadorEquipo);

    /// <summary>
    /// Elimina lógicamente un NadadorEquipo (marca el registro como inactivo
    /// sin borrarlo de la base de datos).
    /// </summary>
    Task<bool> EliminarLogicoAsync(int id);
}