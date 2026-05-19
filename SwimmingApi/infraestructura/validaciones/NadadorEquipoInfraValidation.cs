using SwimmingApi.Application.Interfaces.Repository;

namespace SwimmingApi.Infraestructura.Validaciones;

/// <summary>
/// Validaciones de infraestructura para NadadorEquipo.
/// Comprueba condiciones que requieren acceder a la base de datos,
/// incluyendo la disponibilidad del código único de 6 dígitos.
/// </summary>
public class NadadorEquipoInfraValidation
{
    // Repositorio que consulta los datos de NadadorEquipo.
    private readonly INadadorEquipoRepository _repository;

    /// <summary>
    /// Constructor con inyección de dependencias del repositorio.
    /// </summary>
    public NadadorEquipoInfraValidation(INadadorEquipoRepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Verifica que existe un NadadorEquipo con el ID indicado.
    /// Devuelve true si se encuentra en la base de datos.
    /// </summary>
    public async Task<bool> ExisteAsync(int id)
    {
        var nadadorEquipo = await _repository.ObtenerPorIdAsync(id);
        var resultado = nadadorEquipo != null;
        return resultado;
    }

    /// <summary>
    /// Verifica que el código de 6 dígitos no esté ya en uso por otro NadadorEquipo.
    /// Devuelve true si el código está disponible para asignarse a un nuevo registro.
    /// Se utiliza al generar códigos únicos en el momento de crear la ficha.
    /// </summary>
    public async Task<bool> CodigoDisponibleAsync(int codigo)
    {
        var nadadorEquipo = await _repository.ObtenerPorCodigoAsync(codigo);
        var resultado = nadadorEquipo == null;
        return resultado;
    }
}