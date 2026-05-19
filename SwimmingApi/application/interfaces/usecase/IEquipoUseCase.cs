using SwimmingApi.Application.Dtos.Equipo;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso relacionados con Equipo.
/// Define la lógica de aplicación expuesta a la capa Api,
/// trabajando con DTOs en lugar de entidades de dominio.
/// </summary>
public interface IEquipoUseCase
{
    /// <summary>Obtiene un equipo por su ID.</summary>
    Task<EquipoResponseDto?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene la lista de todos los equipos activos.</summary>
    Task<IEnumerable<EquipoResponseDto>> ObtenerTodosAsync();

    /// <summary>Crea un nuevo equipo a partir de los datos del DTO.</summary>
    Task<EquipoResponseDto> CrearAsync(EquipoRequestDto dto);

    /// <summary>Actualiza un equipo existente con los datos del DTO.</summary>
    Task<EquipoResponseDto> ActualizarAsync(int id, EquipoRequestDto dto);

    /// <summary>Elimina lógicamente un equipo.</summary>
    Task<bool> EliminarAsync(int id);
}