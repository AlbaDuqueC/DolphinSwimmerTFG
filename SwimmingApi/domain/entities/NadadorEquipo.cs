namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Representa la ficha de un nadador dentro de un equipo.
/// La crea el entrenador y existe independientemente de que el nadador
/// tenga o no una cuenta de usuario en el sistema.
/// Si el nadador real se da de alta más adelante, puede vincularse a esta ficha
/// usando el código único que tiene asociado.
/// </summary>
public class NadadorEquipo : EntityBase
{
    /// <summary>Nombre del nadador en el equipo (informativo, lo introduce el entrenador).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del nadador en el equipo (informativo, los introduce el entrenador).</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>
    /// Código único de 6 dígitos generado automáticamente al crear la ficha.
    /// El nadador real lo usa para vincular su cuenta a esta plaza del equipo.
    /// </summary>
    public int Codigo { get; set; }

    /// <summary>Clave foránea del equipo al que pertenece la ficha. Es obligatorio.</summary>
    public int IdEquipo { get; set; }

    /// <summary>
    /// Propiedad de navegación al equipo al que pertenece esta ficha.
    /// Entity Framework la carga automáticamente cuando se incluye en la consulta.
    /// </summary>
    public Equipo Equipo { get; set; } = null!;

    /// <summary>Lista de marcas de tiempo asociadas a esta ficha dentro del equipo.</summary>
    public ICollection<MarcaDeTiempo> ListaDeTiempoEquipo { get; set; } = new List<MarcaDeTiempo>();
}