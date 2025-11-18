using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEnEscenario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_escenarios_ubicaciones_ubicacion_id_int",
                table: "escenarios");

            migrationBuilder.RenameColumn(
                name: "updated_at_timestamp",
                table: "escenarios",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ubicacion_id_int",
                table: "escenarios",
                newName: "ubicacion_id");

            migrationBuilder.RenameColumn(
                name: "descripcion_varChar",
                table: "escenarios",
                newName: "pais");

            migrationBuilder.RenameColumn(
                name: "created_at_timestamp",
                table: "escenarios",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "id_int",
                table: "escenarios",
                newName: "escenario_id");

            migrationBuilder.RenameIndex(
                name: "IX_escenarios_ubicacion_id_int",
                table: "escenarios",
                newName: "IX_escenarios_ubicacion_id");

            migrationBuilder.AddColumn<string>(
                name: "calles",
                table: "escenarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ciudad",
                table: "escenarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "codigo_p",
                table: "escenarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "descripcion",
                table: "escenarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "nombre",
                table: "escenarios",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_escenarios_ubicaciones_ubicacion_id",
                table: "escenarios",
                column: "ubicacion_id",
                principalTable: "ubicaciones",
                principalColumn: "ubicacion_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_escenarios_ubicaciones_ubicacion_id",
                table: "escenarios");

            migrationBuilder.DropColumn(
                name: "calles",
                table: "escenarios");

            migrationBuilder.DropColumn(
                name: "ciudad",
                table: "escenarios");

            migrationBuilder.DropColumn(
                name: "codigo_p",
                table: "escenarios");

            migrationBuilder.DropColumn(
                name: "descripcion",
                table: "escenarios");

            migrationBuilder.DropColumn(
                name: "nombre",
                table: "escenarios");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "escenarios",
                newName: "updated_at_timestamp");

            migrationBuilder.RenameColumn(
                name: "ubicacion_id",
                table: "escenarios",
                newName: "ubicacion_id_int");

            migrationBuilder.RenameColumn(
                name: "pais",
                table: "escenarios",
                newName: "descripcion_varChar");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "escenarios",
                newName: "created_at_timestamp");

            migrationBuilder.RenameColumn(
                name: "escenario_id",
                table: "escenarios",
                newName: "id_int");

            migrationBuilder.RenameIndex(
                name: "IX_escenarios_ubicacion_id",
                table: "escenarios",
                newName: "IX_escenarios_ubicacion_id_int");

            migrationBuilder.AddForeignKey(
                name: "FK_escenarios_ubicaciones_ubicacion_id_int",
                table: "escenarios",
                column: "ubicacion_id_int",
                principalTable: "ubicaciones",
                principalColumn: "ubicacion_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
