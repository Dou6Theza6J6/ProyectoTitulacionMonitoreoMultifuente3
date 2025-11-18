using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEnSensorYMedicion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Updated_At",
                table: "sensores",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "Created_At",
                table: "sensores",
                newName: "created_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "sensores",
                newName: "Updated_At");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "sensores",
                newName: "Created_At");
        }
    }
}
