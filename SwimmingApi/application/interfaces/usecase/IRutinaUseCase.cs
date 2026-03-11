using SwimmingApi.Application.Dtos.Rutina;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso de Rutina.
/// </summary>
public interface IRutinaUseCase
{
    Task<RutinaResponseDto?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<RutinaResponseDto>> ObtenerPorUsuarioAsync(int idUsuario);
    Task<RutinaResponseDto> CrearAsync(RutinaRequestDto dto);
    Task<RutinaResponseDto> ActualizarAsync(int id, RutinaRequestDto dto);
    Task<bool> EliminarAsync(int id);
}
