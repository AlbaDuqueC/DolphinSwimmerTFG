namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa a un entrenador. Hereda de Usuario.
/// </summary>
public class Entrenador : Usuario
{
    /// <summary>FK del equipo que gestiona el entrenador.</summary>
    public int? IdEquipoGestionado { get; set; }

    /// <summary>Equipo que gestiona este entrenador.</summary>
    public Equipo? EquipoGestionado { get; set; }
}
