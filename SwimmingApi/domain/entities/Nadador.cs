namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa a un nadador del sistema.
/// Hereda de Usuario, por lo que tiene los datos básicos de cualquier usuario,
/// y añade la información de su vinculación con un equipo y sus marcas de tiempo.
/// </summary>
public class Nadador : Usuario
{
    /// <summary>
    /// Clave foránea al NadadorEquipo (ficha) al que está vinculado el nadador.
    /// Es nula mientras el nadador no se haya unido a ningún equipo.
    /// </summary>
    public int? IdNadadorEquipo { get; set; }

    /// <summary>
    /// Propiedad de navegación al NadadorEquipo al que está vinculado este nadador.
    /// Representa la "ficha" del nadador dentro del equipo del entrenador.
    /// </summary>
    public NadadorEquipo? NadadorEquipo { get; set; }

    /// <summary>Lista de marcas de tiempo registradas por el nadador. Puede estar vacía.</summary>
    public ICollection<MarcaDeTiempo> ListaDeTiempo { get; set; } = new List<MarcaDeTiempo>();
}