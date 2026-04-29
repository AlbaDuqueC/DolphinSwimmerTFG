namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Registro de un nadador dentro de un equipo.
/// Puede ser creado por un entrenador aunque el nadador no esté registrado en el sistema.
/// Un nadador puede conectarse a este registro para ver sus datos de equipo.
/// </summary>
public class NadadorEquipo : EntityBase
{

    /// <summary>Nombre del nadador en el equipo (para cuando no está registrado en el sistema).</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del nadador en el equipo.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Código único para que un nadador pueda conectarse a este registro.</summary>
    public int Codigo { get; set; }

    /// <summary>FK del equipo al que pertenece. Obligatorio.</summary>
    public int IdEquipo { get; set; }

    /// <summary>Equipo al que pertenece este registro.</summary>
    public Equipo Equipo { get; set; } = null!;

    /// <summary>Lista de marcas de tiempo del nadador en el equipo.</summary>
    public ICollection<MarcaDeTiempo> ListaDeTiempoEquipo { get; set; } = new List<MarcaDeTiempo>();
}
