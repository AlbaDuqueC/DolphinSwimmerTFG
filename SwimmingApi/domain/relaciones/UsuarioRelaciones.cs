using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración de las relaciones y claves foráneas de la entidad Usuario.
/// </summary>
public class UsuarioRelaciones : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Relación Usuario -> Equipo (opcional)
        builder.HasOne(u => u.Equipo)
               .WithMany()
               .HasForeignKey(u => u.IdEquipo)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        // Relación Usuario -> Rutinas (uno a muchos)
        builder.HasMany(u => u.ListaRutinas)
               .WithOne(r => r.Usuario)
               .HasForeignKey(r => r.IdUsuario)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Apellidos).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.PasswordHash).IsRequired();
    }
}
