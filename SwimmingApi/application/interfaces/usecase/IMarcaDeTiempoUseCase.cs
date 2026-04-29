using SwimmingApi.Application.Dtos.MarcaDeTiempo;

namespace SwimmingApi.Application.Interfaces.UseCase;

public interface IMarcaDeTiempoUseCase
{
    Task<MarcaDeTiempoResponseDto?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<MarcaDeTiempoResponseDto>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo);
    Task<IEnumerable<MarcaDeTiempoResponseDto>> ObtenerPorNadadorAsync(int idNadador); // ✨ NUEVO
    Task<MarcaDeTiempoResponseDto> CrearAsync(MarcaDeTiempoRequestDto dto);
    Task<MarcaDeTiempoResponseDto> ActualizarAsync(int id, MarcaDeTiempoRequestDto dto);
    Task<bool> EliminarAsync(int id);
}