using SwimmingApi.Application.Dtos.NadadorEquipo;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso de NadadorEquipo.
/// </summary>
public interface INadadorEquipoUseCase
{
    Task<NadadorEquipoResponseDto?> ObtenerPorIdAsync(int id);
    Task<NadadorEquipoResponseDto?> ObtenerPorCodigoAsync(int codigo);
    Task<IEnumerable<NadadorEquipoResponseDto>> ObtenerPorEquipoAsync(int idEquipo);
    Task<NadadorEquipoResponseDto> CrearAsync(NadadorEquipoRequestDto dto);
    Task<NadadorEquipoResponseDto> ActualizarAsync(int id, NadadorEquipoRequestDto dto);
    Task<bool> EliminarAsync(int id);
}