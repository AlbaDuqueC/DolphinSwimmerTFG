namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad principal que representa a un usuario del sistema.
/// Es la base de Nadador y Entrenador.
/// </summary>
public class Usuario : EntityBase
{
    /// <summary>Nombre del usuario.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del usuario.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Email del usuario. Se usará para autenticación.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>Contraseña encriptada del usuario.</summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>FK al equipo al que pertenece. Puede ser nulo.</summary>
    public int? IdEquipo { get; set; }

    /// <summary>Equipo al que pertenece el usuario.</summary>
    public Equipo? Equipo { get; set; }

    /// <summary>Lista de rutinas del usuario. Puede estar vacía.</summary>
    public ICollection<Rutina> ListaRutinas { get; set; } = new List<Rutina>();
}
