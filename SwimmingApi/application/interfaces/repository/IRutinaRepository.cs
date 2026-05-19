using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Application.Interfaces.Repository;

/// <summary>
/// Contrato para las operaciones de base de datos relacionadas con Rutina.
/// Define las operaciones que cualquier implementación debe respetar.
/// </summary>
public interface IRutinaRepository
{
    /// <summary>Obtiene una rutina por su ID. Devuelve null si no existe.</summary>
    Task<Rutina?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene todas las rutinas asociadas a un usuario concreto.</summary>
    Task<IEnumerable<Rutina>> ObtenerPorUsuarioAsync(int idUsuario);

    /// <summary>Crea una nueva rutina en la base de datos.</summary>
    Task<Rutina> CrearAsync(Rutina rutina);

    /// <summary>Actualiza los datos de una rutina existente.</summary>
    Task<Rutina> ActualizarAsync(Rutina rutina);

    /// <summary>
    /// Elimina lógicamente una rutina (marca el registro como inactivo
    /// sin borrarlo de la base de datos).
    /// </summary>
    Task<bool> EliminarLogicoAsync(int id);
}