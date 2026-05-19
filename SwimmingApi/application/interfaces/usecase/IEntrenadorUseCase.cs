using SwimmingApi.Application.Dtos.Entrenador;

namespace SwimmingApi.Application.Interfaces.UseCase;

/// <summary>
/// Contrato para los casos de uso relacionados con Entrenador.
/// Define la lógica de aplicación expuesta a la capa Api,
/// trabajando con DTOs en lugar de entidades de dominio.
/// </summary>
public interface IEntrenadorUseCase
{
    /// <summary>Obtiene un entrenador por su ID.</summary>
    Task<EntrenadorResponseDto?> ObtenerPorIdAsync(int id);

    /// <summary>Obtiene un entrenador por su correo electrónico (usado al iniciar sesión).</summary>
    Task<EntrenadorResponseDto?> ObtenerPorEmailAsync(string email);

    /// <summary>Obtiene la lista de todos los entrenadores activos.</summary>
    Task<IEnumerable<EntrenadorResponseDto>> ObtenerTodosAsync();

    /// <summary>Crea un nuevo entrenador a partir de los datos del DTO.</summary>
    Task<EntrenadorResponseDto> CrearAsync(EntrenadorRequestDto dto);

    /// <summary>Actualiza un entrenador existente con los datos del DTO.</summary>
    Task<EntrenadorResponseDto> ActualizarAsync(int id, EntrenadorRequestDto dto);

    /// <summary>Elimina lógicamente un entrenador.</summary>
    Task<bool> EliminarAsync(int id);
}