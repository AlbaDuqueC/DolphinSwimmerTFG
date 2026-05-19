namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa una marca de tiempo registrada en una prueba de natación.
/// Puede ser registrada directamente por un nadador para sí mismo,
/// o asignada por un entrenador a uno de los nadadores de su equipo.
/// </summary>
public class MarcaDeTiempo : EntityBase
{
    /// <summary>Tiempo registrado en formato TimeSpan (hh:mm:ss.ms).</summary>
    public TimeSpan Tiempo { get; set; }

    /// <summary>Descripción de la prueba a la que corresponde la marca (por ejemplo: "100m libre").</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>Clave foránea al NadadorEquipo (ficha del equipo) al que pertenece esta marca.</summary>
    public int? IdNadadorEquipo { get; set; }

    /// <summary>Propiedad de navegación al NadadorEquipo al que pertenece esta marca.</summary>
    public NadadorEquipo? NadadorEquipo { get; set; } = null!;

    /// <summary>
    /// Clave foránea del nadador (usuario) que registró la marca.
    /// Es nula si la marca la asignó el entrenador en lugar del propio nadador.
    /// </summary>
    public int? IdNadador { get; set; }

    /// <summary>Propiedad de navegación al nadador que registró la marca.</summary>
    public Nadador? Nadador { get; set; }
}