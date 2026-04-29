using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SwimmingApi.Migrations
{
    /// <inheritdoc />
    public partial class EliminarIdsDuplicados : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NadadoresEquipo_Equipos_IdEquipo",
                table: "NadadoresEquipo");

            migrationBuilder.DropColumn(
                name: "IdEntrenador",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdNadador",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "IdRutina",
                table: "Rutinas");

            migrationBuilder.DropColumn(
                name: "IdNadadorEquipo",
                table: "NadadoresEquipo");

            migrationBuilder.DropColumn(
                name: "IdMarca",
                table: "MarcasDeTiempo");

            migrationBuilder.DropColumn(
                name: "IdEquipo",
                table: "Equipos");

            migrationBuilder.AlterColumn<int>(
                name: "IdNadadorEquipo",
                table: "MarcasDeTiempo",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_NadadoresEquipo_Equipos_IdEquipo",
                table: "NadadoresEquipo",
                column: "IdEquipo",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NadadoresEquipo_Equipos_IdEquipo",
                table: "NadadoresEquipo");

            migrationBuilder.AddColumn<int>(
                name: "IdEntrenador",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdNadador",
                table: "Usuarios",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdRutina",
                table: "Rutinas",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdNadadorEquipo",
                table: "NadadoresEquipo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "IdNadadorEquipo",
                table: "MarcasDeTiempo",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdMarca",
                table: "MarcasDeTiempo",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IdEquipo",
                table: "Equipos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_NadadoresEquipo_Equipos_IdEquipo",
                table: "NadadoresEquipo",
                column: "IdEquipo",
                principalTable: "Equipos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
