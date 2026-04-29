using SwimmingApi.Application.Dtos.Entrenador;

namespace SwimmingApi.Application.Interfaces.UseCase;

public interface IEntrenadorUseCase
{
    Task<EntrenadorResponseDto?> ObtenerPorIdAsync(int id);
    Task<EntrenadorResponseDto?> ObtenerPorEmailAsync(string email); // ✨ NUEVO
    Task<IEnumerable<EntrenadorResponseDto>> ObtenerTodosAsync();
    Task<EntrenadorResponseDto> CrearAsync(EntrenadorRequestDto dto);
    Task<EntrenadorResponseDto> ActualizarAsync(int id, EntrenadorRequestDto dto);
    Task<bool> EliminarAsync(int id);
}