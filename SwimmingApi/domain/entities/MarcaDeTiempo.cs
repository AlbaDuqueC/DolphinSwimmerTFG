namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que registra una marca de tiempo de un nadador.
/// Puede ser registrada por el propio nadador o por un entrenador.
/// </summary>
public class MarcaDeTiempo : EntityBase
{
    /// <summary>Identificador de la marca.</summary>
    public int IdMarca { get; set; }

    /// <summary>Tiempo registrado en formato TimeSpan (hh:mm:ss.ms).</summary>
    public TimeSpan Tiempo { get; set; }

    /// <summary>Descripción o prueba a la que corresponde la marca.</summary>
    public string Descripcion { get; set; } = string.Empty;

    /// <summary>FK al registro NadadorEquipo al que pertenece esta marca.</summary>
    public int IdNadadorEquipo { get; set; }

    /// <summary>Registro NadadorEquipo al que pertenece esta marca.</summary>
    public NadadorEquipo NadadorEquipo { get; set; } = null!;

    /// <summary>FK del nadador que registró la marca. Puede ser nulo si la registró el entrenador.</summary>
    public int? IdNadador { get; set; }

    /// <summary>Nadador que registró la marca.</summary>
    public Nadador? Nadador { get; set; }
}
