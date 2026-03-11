using SwimmingApi.Application.Dtos.Nadador;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso de Nadador.
/// </summary>
public interface INadadorUseCase
{
    Task<NadadorResponseDto?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<NadadorResponseDto>> ObtenerTodosAsync();
    Task<NadadorResponseDto> CrearAsync(NadadorRequestDto dto);
    Task<NadadorResponseDto> ActualizarAsync(int id, NadadorRequestDto dto);
    Task<bool> EliminarAsync(int id);
}
