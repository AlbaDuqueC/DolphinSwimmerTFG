namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa a un nadador. Hereda de Usuario.
/// </summary>
public class Nadador : Usuario
{

    /// <summary>FK al registro del nadador dentro de un equipo. Puede ser nulo.</summary>
    public int? IdNadadorEquipo { get; set; }

    /// <summary>Registro del nadador dentro del equipo.</summary>
    public NadadorEquipo? NadadorEquipo { get; set; }

    /// <summary>Lista de marcas de tiempo del nadador. Puede estar vacía.</summary>
    public ICollection<MarcaDeTiempo> ListaDeTiempo { get; set; } = new List<MarcaDeTiempo>();
}
