using SwimmingApi.Application.Dtos.NadadorEquipo;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso relacionados con NadadorEquipo.
/// Define la lógica de aplicación expuesta a la capa Api,
/// trabajando con DTOs en lugar de entidades de dominio.
/// </summary>
public interface INadadorEquipoUseCase
{
    /// <summary>Obtiene un NadadorEquipo por su ID.</summary>
    Task<NadadorEquipoResponseDto?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene un NadadorEquipo por su código único de 6 dígitos.</summary>
    Task<NadadorEquipoResponseDto?> ObtenerPorCodigoAsync(int codigo);

    /// <summary>Obtiene todos los nadadores registrados en un equipo concreto.</summary>
    Task<IEnumerable<NadadorEquipoResponseDto>> ObtenerPorEquipoAsync(int idEquipo);

    /// <summary>Crea un nuevo NadadorEquipo a partir de los datos del DTO.</summary>
    Task<NadadorEquipoResponseDto> CrearAsync(NadadorEquipoRequestDto dto);

    /// <summary>Actualiza un NadadorEquipo existente con los datos del DTO.</summary>
    Task<NadadorEquipoResponseDto> ActualizarAsync(int id, NadadorEquipoRequestDto dto);

    /// <summary>Elimina lógicamente un NadadorEquipo.</summary>
    Task<bool> EliminarAsync(int id);
}