using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Entrenador.
/// </summary>
public class EntrenadorInfraValidation
{
    private readonly IEntrenadorRepository _repository;

    public EntrenadorInfraValidation(IEntrenadorRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Verifica que el email no está en uso.</summary>
    public async Task<bool> EmailDisponibleAsync(string email)
    {
        var existe = await _repository.ExisteEmailAsync(email);
        var resultado = !existe;
        return resultado;
    }

    /// <summary>Verifica que existe un entrenador con ese ID.</summary>
    public async Task<bool> EntrenadorExisteAsync(int id)
    {
        var entrenador = await _repository.ObtenerPorIdAsync(id);
        var resultado = entrenador != null;
        return resultado;
    }
}
