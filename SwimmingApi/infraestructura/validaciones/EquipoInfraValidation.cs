using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Equipo.
/// </summary>
public class EquipoInfraValidation
{
    private readonly IEquipoRepository _repository;

    public EquipoInfraValidation(IEquipoRepository repository)
    {
        _repository = repository;
    }

    /// <summary>Verifica que existe un equipo con ese ID.</summary>
    public async Task<bool> EquipoExisteAsync(int id)
    {
        var equipo = await _repository.ObtenerPorIdAsync(id);
        var resultado = equipo != null;
        return resultado;
    }
}
