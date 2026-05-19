using Microsoft.EntityFrameworkCore;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Relaciones;

namespace SwimmingApi.Infraestructura.Database;

/// <summary>
/// Contexto principal de Entity Framework para la base de datos.
/// Gestiona todas las entidades del sistema, aplica las configuraciones
/// de relaciones (Fluent API) y define los filtros globales de eliminación lógica.
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// Constructor que recibe las opciones de configuración (cadena de conexión, proveedor, etc.).
    /// Estas opciones se inyectan desde Program.cs al registrar el DbContext.
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Conjuntos de datos (DbSet) que representan las tablas de la base de datos.
    // Cada DbSet permite consultar y modificar los registros de su entidad correspondiente.

    /// <summary>Tabla de usuarios (incluye Nadadores y Entrenadores por herencia TPH).</summary>
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    /// <summary>Vista filtrada de los usuarios que son nadadores.</summary>
    public DbSet<Nadador> Nadadores => Set<Nadador>();

    /// <summary>Vista filtrada de los usuarios que son entrenadores.</summary>
    public DbSet<Entrenador> Entrenadores => Set<Entrenador>();

    /// <summary>Tabla de fichas de nadadores dentro de equipos.</summary>
    public DbSet<NadadorEquipo> NadadoresEquipo => Set<NadadorEquipo>();

    /// <summary>Tabla de equipos.</summary>
    public DbSet<Equipo> Equipos => Set<Equipo>();

    /// <summary>Tabla de rutinas de entrenamiento.</summary>
    public DbSet<Rutina> Rutinas => Set<Rutina>();

    /// <summary>Tabla de marcas de tiempo registradas.</summary>
    public DbSet<MarcaDeTiempo> MarcasDeTiempo => Set<MarcaDeTiempo>();

    /// <summary>
    /// Configura el modelo de la base de datos cuando se crea el contexto.
    /// Aquí se define la estrategia de herencia y se aplican las configuraciones
    /// de las clases de relaciones, así como los filtros globales.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Estrategia de herencia TPH (Table Per Hierarchy):
        // Todas las clases que heredan de Usuario (Nadador, Entrenador) se guardan
        // en la misma tabla. La columna "TipoUsuario" hace de discriminador para
        // saber a qué tipo concreto pertenece cada registro.
        modelBuilder.Entity<Usuario>().HasDiscriminator<string>("TipoUsuario")
            .HasValue<Usuario>("Usuario")
            .HasValue<Nadador>("Nadador")
            .HasValue<Entrenador>("Entrenador");

        // Aplicación de las configuraciones de relaciones definidas con Fluent API.
        // Cada clase contiene las claves foráneas, restricciones e índices de su entidad.
        modelBuilder.ApplyConfiguration(new UsuarioRelaciones());
        modelBuilder.ApplyConfiguration(new NadadorRelaciones());
        modelBuilder.ApplyConfiguration(new NadadorEquipoRelaciones());
        modelBuilder.ApplyConfiguration(new EntrenadorRelaciones());

        // Filtros globales: excluyen automáticamente de todas las consultas
        // los registros eliminados lógicamente (con DeleteAt != null).
        // Así no hay que añadir el filtro manualmente en cada consulta.
        modelBuilder.Entity<Usuario>().HasQueryFilter(u => u.DeleteAt == null);
        modelBuilder.Entity<NadadorEquipo>().HasQueryFilter(ne => ne.DeleteAt == null);
        modelBuilder.Entity<Equipo>().HasQueryFilter(e => e.DeleteAt == null);
        modelBuilder.Entity<Rutina>().HasQueryFilter(r => r.DeleteAt == null);
        modelBuilder.Entity<MarcaDeTiempo>().HasQueryFilter(m => m.DeleteAt == null);
    }
}