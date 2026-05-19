using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración con Fluent API de las relaciones y claves foráneas del Entrenador.
/// Esta clase la utiliza Entity Framework al construir el modelo de la base de datos.
/// </summary>
public class EntrenadorRelaciones : IEntityTypeConfiguration<Entrenador>
{
    /// <summary>
    /// Define las reglas de mapeo de la entidad Entrenador con la base de datos.
    /// </summary>
    public void Configure(EntityTypeBuilder<Entrenador> builder)
    {
        // Relación Entrenador -> Equipo que gestiona.
        // Es opcional: un entrenador puede no tener equipo asignado todavía.
        // Si se elimina el equipo, el IdEquipoGestionado del entrenador se pone a null
        // en lugar de borrarlo en cascada.
        builder.HasOne(e => e.EquipoGestionado)
               .WithMany()
               .HasForeignKey(e => e.IdEquipoGestionado)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
    }
}