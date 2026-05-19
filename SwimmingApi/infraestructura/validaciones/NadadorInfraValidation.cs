using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Nadador.
/// Comprueba condiciones que requieren acceder a la base de datos
/// (existencia del registro, disponibilidad de email, etc.),
/// que no pueden resolverse solo con los datos del DTO.
/// </summary>
public class NadadorInfraValidation
{
    // Repositorio que consulta los datos de los nadadores.
    private readonly INadadorRepository _repository;

    /// <summary>
    /// Constructor con inyección de dependencias del repositorio.
    /// </summary>
    public NadadorInfraValidation(INadadorRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verifica que el email indicado no esté ya registrado por otro nadador.
    /// Devuelve true si el email está disponible para usarse.
    /// </summary>
    public async Task<bool> EmailDisponibleAsync(string email)
    {
        var existe = await _repository.ExisteEmailAsync(email);
        var resultado = !existe;
        return resultado;
    }

    /// <summary>
    /// Verifica que existe un nadador con el ID indicado.
    /// Devuelve true si se encuentra en la base de datos.
    /// </summary>
    public async Task<bool> NadadorExisteAsync(int id)
    {
        var nadador = await _repository.ObtenerPorIdAsync(id);
        var resultado = nadador != null;
        return resultado;
    }
}