using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración de las relaciones y claves foráneas de NadadorEquipo.
/// </summary>
public class NadadorEquipoRelaciones : IEntityTypeConfiguration<NadadorEquipo>
{
    public void Configure(EntityTypeBuilder<NadadorEquipo> builder)
    {
        // Relación NadadorEquipo con Equipo (obligatoria)
        builder.HasOne(ne => ne.Equipo)
               .WithMany(e => e.ListaNadadores)
               .HasForeignKey(ne => ne.IdEquipo)
               .IsRequired()
               .OnDelete(DeleteBehavior.Cascade);

        // Relación NadadorEquipo con MarcasDeTiempo
        builder.HasMany(ne => ne.ListaDeTiempoEquipo)
               .WithOne(m => m.NadadorEquipo)
               .HasForeignKey(m => m.IdNadadorEquipo)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(ne => ne.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(ne => ne.Apellidos).IsRequired().HasMaxLength(150);
        builder.HasIndex(ne => ne.Codigo).IsUnique();
    }
}
