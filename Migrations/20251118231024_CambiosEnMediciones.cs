using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class CambiosEnMediciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status_enum",
                table: "mediciones",
                newName: "status");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "status",
                table: "mediciones",
                newName: "status_enum");
        }
    }
}
