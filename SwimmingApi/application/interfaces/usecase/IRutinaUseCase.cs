using SwimmingApi.Application.Dtos.Rutina;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso relacionados con Rutina.
/// Define la lógica de aplicación expuesta a la capa Api,
/// trabajando con DTOs en lugar de entidades de dominio.
/// </summary>
public interface IRutinaUseCase
{
    /// <summary>Obtiene una rutina por su ID.</summary>
    Task<RutinaResponseDto?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene todas las rutinas asociadas a un usuario concreto.</summary>
    Task<IEnumerable<RutinaResponseDto>> ObtenerPorUsuarioAsync(int idUsuario);

    /// <summary>Crea una nueva rutina a partir de los datos del DTO.</summary>
    Task<RutinaResponseDto> CrearAsync(RutinaRequestDto dto);

    /// <summary>Actualiza una rutina existente con los datos del DTO.</summary>
    Task<RutinaResponseDto> ActualizarAsync(int id, RutinaRequestDto dto);

    /// <summary>Elimina lógicamente una rutina.</summary>
    Task<bool> EliminarAsync(int id);
}