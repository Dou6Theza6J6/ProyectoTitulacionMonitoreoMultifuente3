using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class CorreccionDeModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropForeignKey(
            //     name: "FK_mediciones_escenarios_escenarios_escenario_id_int",
            //     table: "mediciones");

            //  migrationBuilder.DropForeignKey(
            //      name: "FK_mediciones_parametros_parametros_parametro_id_int",
            //      table: "mediciones");

            //  migrationBuilder.DropForeignKey(
            //      name: "FK_mediciones_sensores_sensores_sensor_id_int",
            //      table: "mediciones");

            //   migrationBuilder.DropForeignKey(
            //       name: "FK_mediciones_users_users_id_bigInt",
            //        table: "mediciones");

            //    migrationBuilder.DropForeignKey(
            //       name: "FK_parametros_sensores_sensores_sensor_id_int",
            //        table: "parametros");

            //    migrationBuilder.DropPrimaryKey(
            //        name: "PK_sensores",
            //        table: "sensores");

            //    migrationBuilder.DropColumn(
            //         name: "sensor_id_int",
            //         table: "sensores");

            //    migrationBuilder.DropColumn(
            //        name: "valor_analogico_double",
            //        table: "mediciones");

            //    migrationBuilder.RenameColumn(
            //        name: "tipos_int",
            //        table: "sensores",
            //        newName: "tipo");

            //   migrationBuilder.RenameColumn(
            //     name: "tipo_sensor_int",
            //       table: "sensores",
            //        newName: "sensor_id");

            //   migrationBuilder.RenameColumn(
            //     name: "precio_decimal",
            //   table: "sensores",
            //       newName: "precio");

            //    migrationBuilder.RenameColumn(
            //        name: "nombre_sensor_varChar",
            //        table: "sensores",
            //        newName: "nombre_sensor");

            //    migrationBuilder.RenameColumn(
            //       name: "modelo_varChar",
            //        table: "sensores",
            //        newName: "modelo");

            //    migrationBuilder.RenameColumn(
            //        name: "unidad_medida_varChar",
            //        table: "parametros",
            //        newName: "unidad_medida");

            //    migrationBuilder.RenameColumn(
            //        name: "sensores_sensor_id_int",
            //        table: "parametros",
            //        newName: "sensor_id");

            /*    migrationBuilder.RenameColumn(
                    name: "nombre_parametro_varChar",
                    table: "parametros",
                    newName: "nombre_parametro");

                migrationBuilder.RenameColumn(
                    name: "parametro_id_int",
                    table: "parametros",
                    newName: "parametro_id");

                migrationBuilder.RenameIndex(
                    name: "IX_parametros_sensores_sensor_id_int",
                    table: "parametros",
                    newName: "IX_parametros_sensor_id");

                migrationBuilder.RenameColumn(
                    name: "users_id_bigInt",
                    table: "mediciones",
                    newName: "user_id");

                migrationBuilder.RenameColumn(
                    name: "sensores_sensor_id_int",
                    table: "mediciones",
                    newName: "sensor_id");

                migrationBuilder.RenameColumn(
                    name: "parametros_parametro_id_int",
                    table: "mediciones",
                    newName: "parametro_id");

                migrationBuilder.RenameColumn(
                    name: "fecha_hora_dateTime",
                    table: "mediciones",
                    newName: "updated_at");

                migrationBuilder.RenameColumn(
                    name: "escenarios_escenario_id_int",
                    table: "mediciones",
                    newName: "escenario_id");

                migrationBuilder.RenameColumn(
                    name: "created_by_bigInt",
                    table: "mediciones",
                    newName: "created_by");

                migrationBuilder.RenameColumn(
                    name: "created_at_timestamp",
                    table: "mediciones",
                    newName: "created_at");

                migrationBuilder.RenameIndex(
                    name: "IX_mediciones_users_id_bigInt",
                    table: "mediciones",
                    newName: "IX_mediciones_user_id");

                migrationBuilder.RenameIndex(
                    name: "IX_mediciones_sensores_sensor_id_int",
                    table: "mediciones",
                    newName: "IX_mediciones_sensor_id");

                migrationBuilder.RenameIndex(
                    name: "IX_mediciones_parametros_parametro_id_int",
                    table: "mediciones",
                    newName: "IX_mediciones_parametro_id");

            migrationBuilder.RenameIndex(
                name: "IX_mediciones_escenarios_escenario_id_int",
                table: "mediciones",
                newName: "IX_mediciones_escenario_id");

            migrationBuilder.AlterColumn<int>(
                name: "sensor_id",
                table: "sensores",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "sensores",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_At",
                table: "sensores",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_At",
                table: "parametros",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_At",
                table: "parametros",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_cv_decimal",
                table: "mediciones",
                type: "decimal(10,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_sensores",
                table: "sensores",
                column: "sensor_id");

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_escenarios_escenario_id",
                table: "mediciones",
                column: "escenario_id",
                principalTable: "escenarios",
                principalColumn: "id_int",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_parametros_parametro_id",
                table: "mediciones",
                column: "parametro_id",
                principalTable: "parametros",
                principalColumn: "parametro_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_sensores_sensor_id",
                table: "mediciones",
                column: "sensor_id",
                principalTable: "sensores",
                principalColumn: "sensor_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_users_user_id",
                table: "mediciones",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id_bigInt",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_parametros_sensores_sensor_id",
                table: "parametros",
                column: "sensor_id",
                principalTable: "sensores",
                principalColumn: "sensor_id",
                onDelete: ReferentialAction.Cascade);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_mediciones_escenarios_escenario_id",
                table: "mediciones");

            migrationBuilder.DropForeignKey(
                name: "FK_mediciones_parametros_parametro_id",
                table: "mediciones");

            migrationBuilder.DropForeignKey(
                name: "FK_mediciones_sensores_sensor_id",
                table: "mediciones");

            migrationBuilder.DropForeignKey(
                name: "FK_mediciones_users_user_id",
                table: "mediciones");

            migrationBuilder.DropForeignKey(
                name: "FK_parametros_sensores_sensor_id",
                table: "parametros");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sensores",
                table: "sensores");

            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "sensores");

            migrationBuilder.DropColumn(
                name: "Updated_At",
                table: "sensores");

            migrationBuilder.DropColumn(
                name: "Created_At",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "Updated_At",
                table: "parametros");

            migrationBuilder.DropColumn(
                name: "fecha_hora",
                table: "mediciones");

            migrationBuilder.DropColumn(
                name: "valor_analogico",
                table: "mediciones");

            migrationBuilder.DropColumn(
                name: "valor_digital",
                table: "mediciones");

            migrationBuilder.RenameColumn(
                name: "tipo",
                table: "sensores",
                newName: "tipos_int");

            migrationBuilder.RenameColumn(
                name: "precio",
                table: "sensores",
                newName: "precio_decimal");

            migrationBuilder.RenameColumn(
                name: "nombre_sensor",
                table: "sensores",
                newName: "nombre_sensor_varChar");

            migrationBuilder.RenameColumn(
                name: "modelo",
                table: "sensores",
                newName: "modelo_varChar");

            migrationBuilder.RenameColumn(
                name: "sensor_id",
                table: "sensores",
                newName: "tipo_sensor_int");

            migrationBuilder.RenameColumn(
                name: "unidad_medida",
                table: "parametros",
                newName: "unidad_medida_varChar");

            migrationBuilder.RenameColumn(
                name: "sensor_id",
                table: "parametros",
                newName: "sensores_sensor_id_int");

            migrationBuilder.RenameColumn(
                name: "nombre_parametro",
                table: "parametros",
                newName: "nombre_parametro_varChar");

            migrationBuilder.RenameColumn(
                name: "parametro_id",
                table: "parametros",
                newName: "parametro_id_int");

            migrationBuilder.RenameIndex(
                name: "IX_parametros_sensor_id",
                table: "parametros",
                newName: "IX_parametros_sensores_sensor_id_int");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "mediciones",
                newName: "users_id_bigInt");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "mediciones",
                newName: "fecha_hora_dateTime");

            migrationBuilder.RenameColumn(
                name: "sensor_id",
                table: "mediciones",
                newName: "sensores_sensor_id_int");

            migrationBuilder.RenameColumn(
                name: "parametro_id",
                table: "mediciones",
                newName: "parametros_parametro_id_int");

            migrationBuilder.RenameColumn(
                name: "escenario_id",
                table: "mediciones",
                newName: "escenarios_escenario_id_int");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "mediciones",
                newName: "created_by_bigInt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "mediciones",
                newName: "created_at_timestamp");

            migrationBuilder.RenameIndex(
                name: "IX_mediciones_user_id",
                table: "mediciones",
                newName: "IX_mediciones_users_id_bigInt");

            migrationBuilder.RenameIndex(
                name: "IX_mediciones_sensor_id",
                table: "mediciones",
                newName: "IX_mediciones_sensores_sensor_id_int");

            migrationBuilder.RenameIndex(
                name: "IX_mediciones_parametro_id",
                table: "mediciones",
                newName: "IX_mediciones_parametros_parametro_id_int");

            migrationBuilder.RenameIndex(
                name: "IX_mediciones_escenario_id",
                table: "mediciones",
                newName: "IX_mediciones_escenarios_escenario_id_int");

            migrationBuilder.AlterColumn<int>(
                name: "tipo_sensor_int",
                table: "sensores",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "sensor_id_int",
                table: "sensores",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_cv_decimal",
                table: "mediciones",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,4)",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "valor_analogico_double",
                table: "mediciones",
                type: "double",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_sensores",
                table: "sensores",
                column: "sensor_id_int");

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_escenarios_escenarios_escenario_id_int",
                table: "mediciones",
                column: "escenarios_escenario_id_int",
                principalTable: "escenarios",
                principalColumn: "id_int",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_parametros_parametros_parametro_id_int",
                table: "mediciones",
                column: "parametros_parametro_id_int",
                principalTable: "parametros",
                principalColumn: "parametro_id_int",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_sensores_sensores_sensor_id_int",
                table: "mediciones",
                column: "sensores_sensor_id_int",
                principalTable: "sensores",
                principalColumn: "sensor_id_int",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_mediciones_users_users_id_bigInt",
                table: "mediciones",
                column: "users_id_bigInt",
                principalTable: "users",
                principalColumn: "id_bigInt",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_parametros_sensores_sensores_sensor_id_int",
                table: "parametros",
                column: "sensores_sensor_id_int",
                principalTable: "sensores",
                principalColumn: "sensor_id_int",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
