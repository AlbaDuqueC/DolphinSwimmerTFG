using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SwimmingApi.Domain.Entities;

namespace SwimmingApi.Domain.Relaciones;

/// <summary>
/// Configuración con Fluent API de las relaciones y claves foráneas de Usuario.
/// Como Nadador y Entrenador heredan de Usuario, estas reglas se aplican a ambos.
/// Esta clase la utiliza Entity Framework al construir el modelo de la base de datos.
/// </summary>
public class UsuarioRelaciones : IEntityTypeConfiguration<Usuario>
{
    /// <summary>
    /// Define las reglas de mapeo de la entidad Usuario con la base de datos.
    /// </summary>
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        // Relación Usuario -> Equipo al que pertenece.
        // Es opcional: un usuario puede no pertenecer a ningún equipo todavía.
        // Si el equipo se elimina, el IdEquipo del usuario se pone a null.
        builder.HasOne(u => u.Equipo)
               .WithMany()
               .HasForeignKey(u => u.IdEquipo)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);

        // Relación Usuario -> Lista de rutinas (uno a muchos).
        // Si se elimina el usuario, sus rutinas se eliminan en cascada
        // porque no tiene sentido conservarlas sin su propietario.
        builder.HasMany(u => u.ListaRutinas)
               .WithOne(r => r.Usuario)
               .HasForeignKey(r => r.IdUsuario)
               .OnDelete(DeleteBehavior.Cascade);

        // Restricciones de columnas:
        // Nombre, apellidos, email y password son obligatorios con longitudes máximas.
        builder.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Apellidos).IsRequired().HasMaxLength(150);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);

        // El email debe ser único en toda la tabla para que sirva como
        // identificador inequívoco al iniciar sesión.
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.PasswordHash).IsRequired();
    }
}