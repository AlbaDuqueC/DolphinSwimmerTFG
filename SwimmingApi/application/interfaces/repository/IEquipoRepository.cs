using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos relacionadas con Equipo.
/// Define las operaciones que cualquier implementación debe respetar.
/// </summary>
public interface IEquipoRepository
{
    /// <summary>Obtiene un equipo por su ID. Devuelve null si no existe.</summary>
    Task<Equipo?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene la lista de todos los equipos activos.</summary>
    Task<IEnumerable<Equipo>> ObtenerTodosAsync();

    /// <summary>Crea un nuevo equipo en la base de datos.</summary>
    Task<Equipo> CrearAsync(Equipo equipo);

    /// <summary>Actualiza los datos de un equipo existente.</summary>
    Task<Equipo> ActualizarAsync(Equipo equipo);

    /// <summary>
    /// Elimina lógicamente un equipo (marca el registro como inactivo
    /// sin borrarlo de la base de datos).
    /// </summary>
    Task<bool> EliminarLogicoAsync(int id);
}