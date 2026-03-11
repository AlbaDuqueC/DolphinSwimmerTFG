using SwimmingApi.Application.Dtos.MarcaDeTiempo;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso de MarcaDeTiempo.
/// </summary>
public interface IMarcaDeTiempoUseCase
{
    Task<MarcaDeTiempoResponseDto?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<MarcaDeTiempoResponseDto>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo);
    Task<MarcaDeTiempoResponseDto> CrearAsync(MarcaDeTiempoRequestDto dto);
    Task<MarcaDeTiempoResponseDto> ActualizarAsync(int id, MarcaDeTiempoRequestDto dto);
    Task<bool> EliminarAsync(int id);
}
