using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para MarcaDeTiempo.
/// </summary>
public class MarcaDeTiempoInfraValidation
{
    private readonly IMarcaRepository _repository;

    public MarcaDeTiempoInfraValidation(IMarcaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Verifica que existe una marca con ese ID.</summary>
    public async Task<bool> MarcaExisteAsync(int id)
    {
        var marca = await _repository.ObtenerPorIdAsync(id);
        var resultado = marca != null;
        return resultado;
    }
}
