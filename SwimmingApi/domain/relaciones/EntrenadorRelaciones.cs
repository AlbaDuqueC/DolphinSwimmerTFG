using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración de las relaciones y claves foráneas del Entrenador.
/// </summary>
public class EntrenadorRelaciones : IEntityTypeConfiguration<Entrenador>
{
    public void Configure(EntityTypeBuilder<Entrenador> builder)
    {
        // Relación Entrenador -> Equipo que gestiona (opcional)
        builder.HasOne(e => e.EquipoGestionado)
               .WithMany()
               .HasForeignKey(e => e.IdEquipoGestionado)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
    }
}
