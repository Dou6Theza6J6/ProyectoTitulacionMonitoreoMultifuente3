using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEnUbi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at_timestamp",
                table: "ubicaciones",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "nombre_ubicacion_varChar",
                table: "ubicaciones",
                newName: "nombre");

            migrationBuilder.RenameColumn(
                name: "longitud_double",
                table: "ubicaciones",
                newName: "longitud");

            migrationBuilder.RenameColumn(
                name: "latitud_double",
                table: "ubicaciones",
                newName: "latitud");

            migrationBuilder.RenameColumn(
                name: "created_at_timestamp",
                table: "ubicaciones",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ubicacion_id_int",
                table: "ubicaciones",
                newName: "ubicacion_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "ubicaciones",
                newName: "updated_at_timestamp");

            migrationBuilder.RenameColumn(
                name: "nombre",
                table: "ubicaciones",
                newName: "nombre_ubicacion_varChar");

            migrationBuilder.RenameColumn(
                name: "longitud",
                table: "ubicaciones",
                newName: "longitud_double");

            migrationBuilder.RenameColumn(
                name: "latitud",
                table: "ubicaciones",
                newName: "latitud_double");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "ubicaciones",
                newName: "created_at_timestamp");

            migrationBuilder.RenameColumn(
                name: "ubicacion_id",
                table: "ubicaciones",
                newName: "ubicacion_id_int");
        }
    }
}
