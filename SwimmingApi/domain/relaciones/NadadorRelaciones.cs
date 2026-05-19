using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración con Fluent API de las relaciones y claves foráneas de Nadador.
/// Esta clase la utiliza Entity Framework al construir el modelo de la base de datos.
/// </summary>
public class NadadorRelaciones : IEntityTypeConfiguration<Nadador>
{
    /// <summary>
    /// Define las reglas de mapeo de la entidad Nadador con la base de datos.
    /// </summary>
    public void Configure(EntityTypeBuilder<Nadador> builder)
    {
        // Relación Nadador -> NadadorEquipo (ficha del equipo).
        // Es opcional: un nadador puede no estar todavía vinculado a un equipo.
        // Si se elimina la ficha, el IdNadadorEquipo del nadador se pone a null.
        builder.HasOne(n => n.NadadorEquipo)
               .WithMany()
               .HasForeignKey(n => n.IdNadadorEquipo)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        // Relación Nadador -> Lista de marcas de tiempo (uno a muchos).
        // Es opcional porque las marcas también pueden estar creadas por el entrenador.
        // Si se elimina el nadador, el IdNadador de las marcas se pone a null
        // (las marcas no se eliminan, solo pierden la referencia).
        builder.HasMany(n => n.ListaDeTiempo)
               .WithOne(m => m.Nadador)
               .HasForeignKey(m => m.IdNadador)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
    }
}