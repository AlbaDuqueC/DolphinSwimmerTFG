using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Nadador.
/// Comprueba condiciones que requieren acceso a la base de datos.
/// </summary>
public class NadadorInfraValidation
{
    private readonly INadadorRepository _repository;

    public NadadorInfraValidation(INadadorRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Verifica que el email no está en uso por otro nadador.</summary>
    public async Task<bool> EmailDisponibleAsync(string email)
    {
        var existe = await _repository.ExisteEmailAsync(email);
        var resultado = !existe;
        return resultado;
    }

    /// <summary>Verifica que existe un nadador con ese ID.</summary>
    public async Task<bool> NadadorExisteAsync(int id)
    {
        var nadador = await _repository.ObtenerPorIdAsync(id);
        var resultado = nadador != null;
        return resultado;
    }
}
