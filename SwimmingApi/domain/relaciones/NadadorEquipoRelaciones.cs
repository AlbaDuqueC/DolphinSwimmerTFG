using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración con Fluent API de las relaciones y claves foráneas de NadadorEquipo.
/// Esta clase la utiliza Entity Framework al construir el modelo de la base de datos.
/// </summary>
public class NadadorEquipoRelaciones : IEntityTypeConfiguration<NadadorEquipo>
{
    /// <summary>
    /// Define las reglas de mapeo de la entidad NadadorEquipo con la base de datos.
    /// </summary>
    public void Configure(EntityTypeBuilder<NadadorEquipo> builder)
    {
        // Relación NadadorEquipo -> Equipo al que pertenece.
        // Si el equipo se elimina, el IdEquipo del NadadorEquipo se pone a null
        // en lugar de borrarlo en cascada (preserva el histórico).
        builder.HasOne(ne => ne.Equipo)
               .WithMany(e => e.ListaNadadores)
               .HasForeignKey(ne => ne.IdEquipo)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        // Relación NadadorEquipo -> Lista de marcas de tiempo (uno a muchos).
        // Si se elimina el NadadorEquipo, sus marcas se eliminan en cascada.
        builder.HasMany(ne => ne.ListaDeTiempoEquipo)
               .WithOne(m => m.NadadorEquipo)
               .HasForeignKey(m => m.IdNadadorEquipo)
               .OnDelete(DeleteBehavior.Cascade);

        // Restricciones de columnas:
        // Nombre y apellidos son obligatorios y tienen una longitud máxima.
        builder.Property(ne => ne.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(ne => ne.Apellidos).IsRequired().HasMaxLength(150);

        // El código de 6 dígitos debe ser único en toda la tabla,
        // ya que es la forma de identificar a un nadador del equipo.
        builder.HasIndex(ne => ne.Codigo).IsUnique();
    }
}