namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad principal que representa a un usuario del sistema.
/// Contiene los datos comunes a todos los usuarios (nombre, email, contraseña, foto...)
/// y actúa como clase base de Nadador y Entrenador.
/// </summary>
public class Usuario : EntityBase
{
    /// <summary>Nombre del usuario.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>Apellidos del usuario.</summary>
    public string Apellidos { get; set; } = string.Empty;

    /// <summary>Correo electrónico del usuario. Se utiliza como identificador para autenticación.</summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contraseña del usuario en formato encriptado (hash BCrypt).
    /// Nunca se almacena en texto plano por seguridad.
    /// </summary>
    public string PasswordHash { get; set; } = string.Empty;

    /// <summary>
    /// URL de la foto de perfil del usuario, almacenada en Firebase Storage.
    /// Nula si el usuario no ha subido ninguna foto.
    /// </summary>
    public string? FotoPerfil { get; set; }

    /// <summary>Clave foránea al equipo al que pertenece el usuario. Puede ser nula si todavía no pertenece a ninguno.</summary>
    public int? IdEquipo { get; set; }

    /// <summary>
    /// Propiedad de navegación al equipo al que pertenece el usuario.
    /// Entity Framework la carga automáticamente cuando se incluye en la consulta.
    /// </summary>
    public Equipo? Equipo { get; set; }

    /// <summary>Lista de rutinas que ha creado el usuario. Puede estar vacía.</summary>
    public ICollection<Rutina> ListaRutinas { get; set; } = new List<Rutina>();
}
