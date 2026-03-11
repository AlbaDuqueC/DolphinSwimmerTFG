using SwimmingApi.Application.Dtos.Entrenador;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso de Entrenador.
/// </summary>
public interface IEntrenadorUseCase
{
    Task<EntrenadorResponseDto?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<EntrenadorResponseDto>> ObtenerTodosAsync();
    Task<EntrenadorResponseDto> CrearAsync(EntrenadorRequestDto dto);
    Task<EntrenadorResponseDto> ActualizarAsync(int id, EntrenadorRequestDto dto);
    Task<bool> EliminarAsync(int id);
}
