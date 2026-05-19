namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa a un entrenador del sistema.
/// Hereda de Usuario, por lo que tiene los datos básicos de cualquier usuario
/// y añade la información del equipo que gestiona.
/// </summary>
public class Entrenador : Usuario
{
    /// <summary>
    /// Clave foránea al equipo que gestiona el entrenador.
    /// Es nula mientras el entrenador no haya creado su equipo todavía.
    /// </summary>
    public int? IdEquipoGestionado { get; set; }

    /// <summary>
    /// Propiedad de navegación al equipo que gestiona este entrenador.
    /// Entity Framework la carga automáticamente cuando se incluye en la consulta.
    /// </summary>
    public Equipo? EquipoGestionado { get; set; }
}