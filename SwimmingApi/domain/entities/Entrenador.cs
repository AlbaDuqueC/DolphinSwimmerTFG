namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa a un entrenador. Hereda de Usuario.
/// </summary>
public class Entrenador : Usuario
{
    /// <summary>Identificador específico del entrenador para distinguirlo de nadadores.</summary>
    public int IdEntrenador { get; set; }

    /// <summary>FK del equipo que gestiona el entrenador.</summary>
    public int? IdEquipoGestionado { get; set; }

    /// <summary>Equipo que gestiona este entrenador.</summary>
    public Equipo? EquipoGestionado { get; set; }
}
