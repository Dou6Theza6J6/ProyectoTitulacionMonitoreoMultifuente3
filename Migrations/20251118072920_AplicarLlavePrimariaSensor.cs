using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class AplicarLlavePrimariaSensor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
       /*     migrationBuilder.DropForeignKey(
                name: "FK_mediciones_sensores_sensor_id",
                table: "mediciones");

            migrationBuilder.DropForeignKey(
                name: "FK_parametros_sensores_sensor_id",
                table: "parametros");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_sensores_TempId",
                table: "sensores");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_sensores_TempId1",
                table: "sensores");

            migrationBuilder.DropColumn(
                name: "TempId",
                table: "sensores");

            migrationBuilder.DropColumn(
                name: "TempId1",
                table: "sensores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sensores",
                table: "sensores",
                column: "sensor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_sensores_sensor_id",
                table: "mediciones",
                column: "sensor_id",
                principalTable: "sensores",
                principalColumn: "sensor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_parametros_sensores_sensor_id",
                table: "parametros",
                column: "sensor_id",
                principalTable: "sensores",
                principalColumn: "sensor_id",
                onDelete: ReferentialAction.Cascade); */
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mediciones_sensores_sensor_id",
                table: "mediciones");

            migrationBuilder.DropForeignKey(
                name: "FK_parametros_sensores_sensor_id",
                table: "parametros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sensores",
                table: "sensores");

            migrationBuilder.AddColumn<int>(
                name: "TempId",
                table: "sensores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TempId1",
                table: "sensores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_sensores_TempId",
                table: "sensores",
                column: "TempId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_sensores_TempId1",
                table: "sensores",
                column: "TempId1");

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_sensores_sensor_id",
                table: "mediciones",
                column: "sensor_id",
                principalTable: "sensores",
                principalColumn: "TempId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_parametros_sensores_sensor_id",
                table: "parametros",
                column: "sensor_id",
                principalTable: "sensores",
                principalColumn: "TempId1",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
