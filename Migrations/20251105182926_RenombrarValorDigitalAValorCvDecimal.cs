using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class RenombrarValorDigitalAValorCvDecimal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "valor_digital_float",
                table: "mediciones");

            migrationBuilder.AddColumn<decimal>(
                name: "valor_cv_decimal",
                table: "mediciones",
                type: "decimal(65,30)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "valor_cv_decimal",
                table: "mediciones");

            migrationBuilder.AddColumn<float>(
                name: "valor_digital_float",
                table: "mediciones",
                type: "float",
                nullable: true);
        }
    }
}
