using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Rutina.
/// Comprueba condiciones que requieren acceder a la base de datos.
/// </summary>
public class RutinaInfraValidation
{
    // Repositorio que consulta los datos de las rutinas.
    private readonly IRutinaRepository _repository;

    /// <summary>
    /// Constructor con inyección de dependencias del repositorio.
    /// </summary>
    public RutinaInfraValidation(IRutinaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verifica que existe una rutina con el ID indicado.
    /// Devuelve true si se encuentra en la base de datos.
    /// </summary>
    public async Task<bool> RutinaExisteAsync(int id)
    {
        var rutina = await _repository.ObtenerPorIdAsync(id);
        var resultado = rutina != null;
        return resultado;
    }
}