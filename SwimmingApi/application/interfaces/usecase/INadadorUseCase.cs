using SwimmingApi.Application.Dtos.Nadador;

namespace SwimmingApi.Application.Interfaces.UseCase;

public interface INadadorUseCase
{
    Task<NadadorResponseDto?> ObtenerPorIdAsync(int id);

    Task<NadadorResponseDto?> ObtenerPorEmailAsync(string email);
    Task<IEnumerable<NadadorResponseDto>> ObtenerTodosAsync();
    Task<NadadorResponseDto> CrearAsync(NadadorRequestDto dto);
    Task<NadadorResponseDto> ActualizarAsync(int id, NadadorRequestDto dto);
    Task<NadadorResponseDto> VincularConCodigoAsync(int idNadador, int codigo);
    Task<bool> EliminarAsync(int id);
}