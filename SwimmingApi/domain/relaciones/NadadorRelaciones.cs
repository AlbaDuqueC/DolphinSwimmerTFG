using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración de las relaciones y claves foráneas de la entidad Nadador.
/// </summary>
public class NadadorRelaciones : IEntityTypeConfiguration<Nadador>
{
    public void Configure(EntityTypeBuilder<Nadador> builder)
    {
        // Relación Nadador -> NadadorEquipo (opcional)
        builder.HasOne(n => n.NadadorEquipo)
               .WithMany()
               .HasForeignKey(n => n.IdNadadorEquipo)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        // Relación Nadador -> MarcasDeTiempo (uno a muchos)
        builder.HasMany(n => n.ListaDeTiempo)
               .WithOne(m => m.Nadador)
               .HasForeignKey(m => m.IdNadador)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
