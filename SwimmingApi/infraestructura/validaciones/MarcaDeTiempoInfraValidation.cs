using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para MarcaDeTiempo.
/// Comprueba condiciones que requieren acceder a la base de datos.
/// </summary>
public class MarcaDeTiempoInfraValidation
{
    // Repositorio que consulta los datos de las marcas de tiempo.
    private readonly IMarcaRepository _repository;

    /// <summary>
    /// Constructor con inyección de dependencias del repositorio.
    /// </summary>
    public MarcaDeTiempoInfraValidation(IMarcaRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verifica que existe una marca de tiempo con el ID indicado.
    /// Devuelve true si se encuentra en la base de datos.
    /// </summary>
    public async Task<bool> MarcaExisteAsync(int id)
    {
        var marca = await _repository.ObtenerPorIdAsync(id);
        var resultado = marca != null;
        return resultado;
    }
}