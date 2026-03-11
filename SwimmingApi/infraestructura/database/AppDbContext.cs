using Microsoft.EntityFrameworkCore;
using SwimmingApi.Domain.Entities;
using SwimmingApi.Domain.Relaciones;

namespace SwimmingApi.Infraestructura.Database;

/// <summary>
/// Contexto principal de la base de datos.
/// Gestiona todas las entidades y aplica las configuraciones de relaciones.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Nadador> Nadadores => Set<Nadador>();
    public DbSet<Entrenador> Entrenadores => Set<Entrenador>();
    public DbSet<NadadorEquipo> NadadoresEquipo => Set<NadadorEquipo>();
    public DbSet<Equipo> Equipos => Set<Equipo>();
    public DbSet<Rutina> Rutinas => Set<Rutina>();
    public DbSet<MarcaDeTiempo> MarcasDeTiempo => Set<MarcaDeTiempo>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Herencia TPH (Table Per Hierarchy) para Usuario
        modelBuilder.Entity<Usuario>().HasDiscriminator<string>("TipoUsuario")
            .HasValue<Usuario>("Usuario")
            .HasValue<Nadador>("Nadador")
            .HasValue<Entrenador>("Entrenador");

        // Aplicar configuraciones de relaciones
        modelBuilder.ApplyConfiguration(new UsuarioRelaciones());
        modelBuilder.ApplyConfiguration(new NadadorRelaciones());
        modelBuilder.ApplyConfiguration(new NadadorEquipoRelaciones());
        modelBuilder.ApplyConfiguration(new EntrenadorRelaciones());

        // Filtro global: excluir entidades eliminadas lógicamente
        modelBuilder.Entity<Usuario>().HasQueryFilter(u => u.DeleteAt == null);
        modelBuilder.Entity<NadadorEquipo>().HasQueryFilter(ne => ne.DeleteAt == null);
        modelBuilder.Entity<Equipo>().HasQueryFilter(e => e.DeleteAt == null);
        modelBuilder.Entity<Rutina>().HasQueryFilter(r => r.DeleteAt == null);
        modelBuilder.Entity<MarcaDeTiempo>().HasQueryFilter(m => m.DeleteAt == null);
    }
}
