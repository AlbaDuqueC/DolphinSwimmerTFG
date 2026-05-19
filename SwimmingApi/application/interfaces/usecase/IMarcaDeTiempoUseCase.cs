using SwimmingApi.Application.Dtos.MarcaDeTiempo;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso relacionados con MarcaDeTiempo.
/// Define la lógica de aplicación expuesta a la capa Api,
/// trabajando con DTOs en lugar de entidades de dominio.
/// </summary>
public interface IMarcaDeTiempoUseCase
{
    /// <summary>Obtiene una marca de tiempo por su ID.</summary>
    Task<MarcaDeTiempoResponseDto?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene todas las marcas asociadas a un NadadorEquipo concreto.</summary>
    Task<IEnumerable<MarcaDeTiempoResponseDto>> ObtenerPorNadadorEquipoAsync(int idNadadorEquipo);

    /// <summary>Obtiene todas las marcas registradas por un nadador.</summary>
    Task<IEnumerable<MarcaDeTiempoResponseDto>> ObtenerPorNadadorAsync(int idNadador);

    /// <summary>Crea una nueva marca de tiempo a partir de los datos del DTO.</summary>
    Task<MarcaDeTiempoResponseDto> CrearAsync(MarcaDeTiempoRequestDto dto);

    /// <summary>Actualiza una marca de tiempo existente con los datos del DTO.</summary>
    Task<MarcaDeTiempoResponseDto> ActualizarAsync(int id, MarcaDeTiempoRequestDto dto);

    /// <summary>Elimina lógicamente una marca de tiempo.</summary>
    Task<bool> EliminarAsync(int id);
}