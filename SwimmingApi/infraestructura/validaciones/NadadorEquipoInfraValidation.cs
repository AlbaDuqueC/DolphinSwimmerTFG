using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para NadadorEquipo.
/// </summary>
public class NadadorEquipoInfraValidation
{
    private readonly INadadorEquipoRepository _repository;

    public NadadorEquipoInfraValidation(INadadorEquipoRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Verifica que existe un NadadorEquipo con ese ID.</summary>
    public async Task<bool> ExisteAsync(int id)
    {
        var nadadorEquipo = await _repository.ObtenerPorIdAsync(id);
        var resultado = nadadorEquipo != null;
        return resultado;
    }

    /// <summary>Verifica que el código no está en uso.</summary>
    public async Task<bool> CodigoDisponibleAsync(int codigo)
    {
        var nadadorEquipo = await _repository.ObtenerPorCodigoAsync(codigo);
        var resultado = nadadorEquipo == null;
        return resultado;
    }
}
