using SwimmingApi.Application.Dtos.Equipo;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso de Equipo.
/// </summary>
public interface IEquipoUseCase
{
    Task<EquipoResponseDto?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<EquipoResponseDto>> ObtenerTodosAsync();
    Task<EquipoResponseDto> CrearAsync(EquipoRequestDto dto);
    Task<EquipoResponseDto> ActualizarAsync(int id, EquipoRequestDto dto);
    Task<bool> EliminarAsync(int id);
}
