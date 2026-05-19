using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para Equipo.
/// Comprueba condiciones que requieren acceder a la base de datos.
/// </summary>
public class EquipoInfraValidation
{
    // Repositorio que consulta los datos de los equipos.
    private readonly IEquipoRepository _repository;

    /// <summary>
    /// Constructor con inyección de dependencias del repositorio.
    /// </summary>
    public EquipoInfraValidation(IEquipoRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verifica que existe un equipo con el ID indicado.
    /// Devuelve true si se encuentra en la base de datos.
    /// </summary>
    public async Task<bool> EquipoExisteAsync(int id)
    {
        var equipo = await _repository.ObtenerPorIdAsync(id);
        var resultado = equipo != null;
        return resultado;
    }
}