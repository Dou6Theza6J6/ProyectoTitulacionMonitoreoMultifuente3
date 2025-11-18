using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class ADDCOLUMN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_hora",
                table: "mediciones",
                type: "datetime",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "valor_analogico",
                table: "mediciones",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "valor_digital",
                table: "mediciones",
                type: "double",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fecha_hora",
                table: "mediciones");

            migrationBuilder.DropColumn(
                name: "valor_analogico",
                table: "mediciones");

            migrationBuilder.DropColumn(
                name: "valor_digital",
                table: "mediciones");
        }
    }
}
