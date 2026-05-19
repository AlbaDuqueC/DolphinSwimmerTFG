using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Entrenador.
/// Comprueba condiciones que requieren acceder a la base de datos
/// (existencia del registro, disponibilidad de email, etc.),
/// que no pueden resolverse solo con los datos del DTO.
/// </summary>
public class EntrenadorInfraValidation
{
    // Repositorio que consulta los datos de los entrenadores.
    private readonly IEntrenadorRepository _repository;

    /// <summary>
    /// Constructor con inyección de dependencias del repositorio.
    /// </summary>
    public EntrenadorInfraValidation(IEntrenadorRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verifica que el email indicado no esté ya registrado por otro entrenador.
    /// Devuelve true si el email está disponible para usarse.
    /// </summary>
    public async Task<bool> EmailDisponibleAsync(string email)
    {
        var existe = await _repository.ExisteEmailAsync(email);
        var resultado = !existe;
        return resultado;
    }

    /// <summary>
    /// Verifica que existe un entrenador con el ID indicado.
    /// Devuelve true si se encuentra en la base de datos.
    /// </summary>
    public async Task<bool> EntrenadorExisteAsync(int id)
    {
        var entrenador = await _repository.ObtenerPorIdAsync(id);
        var resultado = entrenador != null;
        return resultado;
    }
}