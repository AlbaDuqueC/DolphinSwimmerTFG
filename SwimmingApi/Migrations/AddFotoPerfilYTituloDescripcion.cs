using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwimmingApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFotoPerfilYTituloDescripcion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Añade la columna FotoPerfil a la tabla de usuarios.
            // Es nullable porque los usuarios existentes no tienen foto.
            migrationBuilder.AddColumn<string>(
                name: "FotoPerfil",
                table: "Usuarios",
                type: "text",
                nullable: true);

            // Añade la columna Titulo a la tabla de rutinas.
            // Se inicializa con el valor de Contenido para que los registros
            // existentes no queden con el título vacío.
            migrationBuilder.AddColumn<string>(
                name: "Titulo",
                table: "Rutinas",
                type: "text",
                nullable: false,
                defaultValue: "");

            // Copia el contenido existente al título para los registros anteriores.
            migrationBuilder.Sql("UPDATE \"Rutinas\" SET \"Titulo\" = \"Contenido\" WHERE \"Titulo\" = ''");

            // Añade la columna Descripcion a la tabla de rutinas.
            // Es nullable porque los eventos existentes no tienen descripción.
            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "Rutinas",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revierte los cambios eliminando las columnas añadidas.
            migrationBuilder.DropColumn(
                name: "FotoPerfil",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Titulo",
                table: "Rutinas");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "Rutinas");
        }
    }
}
