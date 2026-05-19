namespace SwimmingApi.Domain.Entities;

/// <summary>
/// Entidad que representa un equipo de natación.
/// Cada equipo es creado y gestionado por un entrenador, y contiene
/// los nadadores registrados (NadadorEquipo) que forman parte de él.
/// </summary>
public class Equipo : EntityBase
{
    /// <summary>Nombre del equipo.</summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Lista de nadadores (fichas) registrados en el equipo.
    /// Cada elemento es un NadadorEquipo creado por el entrenador,
    /// que puede o no estar vinculado a una cuenta de usuario real.
    /// </summary>
    public ICollection<NadadorEquipo> ListaNadadores { get; set; } = new List<NadadorEquipo>();
}