using SwimmingApi.Application.Dtos.Nadador;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso relacionados con Nadador.
/// Define la lógica de aplicación expuesta a la capa Api,
/// trabajando con DTOs en lugar de entidades de dominio.
/// </summary>
public interface INadadorUseCase
{
    /// <summary>Obtiene un nadador por su ID.</summary>
    Task<NadadorResponseDto?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene un nadador por su correo electrónico (usado al iniciar sesión).</summary>
    Task<NadadorResponseDto?> ObtenerPorEmailAsync(string email);

    /// <summary>Obtiene la lista de todos los nadadores activos.</summary>
    Task<IEnumerable<NadadorResponseDto>> ObtenerTodosAsync();

    /// <summary>Crea un nuevo nadador a partir de los datos del DTO.</summary>
    Task<NadadorResponseDto> CrearAsync(NadadorRequestDto dto);

    /// <summary>Actualiza un nadador existente con los datos del DTO.</summary>
    Task<NadadorResponseDto> ActualizarAsync(int id, NadadorRequestDto dto);

    /// <summary>
    /// Vincula un nadador con un NadadorEquipo dentro de un equipo,
    /// utilizando el código único de 6 dígitos que le proporciona el entrenador.
    /// </summary>
    Task<NadadorResponseDto> VincularConCodigoAsync(int idNadador, int codigo);

    /// <summary>Elimina lógicamente un nadador.</summary>
    Task<bool> EliminarAsync(int id);
}