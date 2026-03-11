using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Rutina.
/// </summary>
public class RutinaInfraValidation
{
    private readonly IRutinaRepository _repository;

    public RutinaInfraValidation(IRutinaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Verifica que existe una rutina con ese ID.</summary>
    public async Task<bool> RutinaExisteAsync(int id)
    {
        var rutina = await _repository.ObtenerPorIdAsync(id);
        var resultado = rutina != null;
        return resultado;
    }
}
