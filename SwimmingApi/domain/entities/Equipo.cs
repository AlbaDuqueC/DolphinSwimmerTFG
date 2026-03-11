namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa un equipo de natación.
/// Puede ser creado por un entrenador.
/// </summary>
public class Equipo : EntityBase
{
    /// <summary>Identificador del equipo.</summary>
    public int IdEquipo { get; set; }

    /// <summary>Nombre del equipo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Lista de nadadores registrados en el equipo.</summary>
    public ICollection<NadadorEquipo> ListaNadadores { get; set; } = new List<NadadorEquipo>();
}
