using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoreoMultifuente3.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusAndCvToMediciones : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permissions_Permissionsid_int",
                table: "role_permission");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "name_varChar",
                keyValue: null,
                column: "name_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "name_varChar",
                table: "users",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "sessions",
                keyColumn: "user_agent_text",
                keyValue: null,
                column: "user_agent_text",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "user_agent_text",
                table: "sessions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "sessions",
                keyColumn: "payload_longtext",
                keyValue: null,
                column: "payload_longtext",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "payload_longtext",
                table: "sessions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "sessions",
                keyColumn: "ip_address_varChar",
                keyValue: null,
                column: "ip_address_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "ip_address_varChar",
                table: "sessions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "nombre_sensor_varChar",
                table: "sensores",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "modelo_varChar",
                table: "sensores",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "tipo_sensor_int",
                table: "sensores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Permissionsid_int",
                table: "role_permission",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "slug_varChar",
                keyValue: null,
                column: "slug_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "slug_varChar",
                table: "permissions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "permissions",
                keyColumn: "name_varChar",
                keyValue: null,
                column: "name_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "name_varChar",
                table: "permissions",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "password_reset_tokens",
                keyColumn: "token_varChar",
                keyValue: null,
                column: "token_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "token_varChar",
                table: "password_reset_tokens",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "migrations",
                keyColumn: "migration_varChar",
                keyValue: null,
                column: "migration_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "migration_varChar",
                table: "migrations",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<float>(
                name: "valor_digital_float",
                table: "mediciones",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "jobs",
                keyColumn: "queue_varChar",
                keyValue: null,
                column: "queue_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "queue_varChar",
                table: "jobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "jobs",
                keyColumn: "payload_longtext",
                keyValue: null,
                column: "payload_longtext",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "payload_longtext",
                table: "jobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "job_batches",
                keyColumn: "name_varChar",
                keyValue: null,
                column: "name_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "name_varChar",
                table: "job_batches",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "job_batches",
                keyColumn: "failed_jobs_ids_longtext",
                keyValue: null,
                column: "failed_jobs_ids_longtext",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "failed_jobs_ids_longtext",
                table: "job_batches",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "job_batches",
                keyColumn: "canceled_at_int",
                keyValue: null,
                column: "canceled_at_int",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "canceled_at_int",
                table: "job_batches",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "failed_jobs",
                keyColumn: "uuid_varChar",
                keyValue: null,
                column: "uuid_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "uuid_varChar",
                table: "failed_jobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "failed_jobs",
                keyColumn: "queue_text",
                keyValue: null,
                column: "queue_text",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "queue_text",
                table: "failed_jobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "failed_jobs",
                keyColumn: "payload_longtext",
                keyValue: null,
                column: "payload_longtext",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "payload_longtext",
                table: "failed_jobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "failed_jobs",
                keyColumn: "exception_longtext",
                keyValue: null,
                column: "exception_longtext",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "exception_longtext",
                table: "failed_jobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "failed_jobs",
                keyColumn: "connection_text",
                keyValue: null,
                column: "connection_text",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "connection_text",
                table: "failed_jobs",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "cache_locks",
                keyColumn: "owner_varChar",
                keyValue: null,
                column: "owner_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "owner_varChar",
                table: "cache_locks",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "cache",
                keyColumn: "value_mediumtext",
                keyValue: null,
                column: "value_mediumtext",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "value_mediumtext",
                table: "cache",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permissions_Permissionsid_int",
                table: "role_permission",
                column: "Permissionsid_int",
                principalTable: "permissions",
                principalColumn: "id_int",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_role_permission_permissions_Permissionsid_int",
                table: "role_permission");

            migrationBuilder.DropColumn(
                name: "tipo_sensor_int",
                table: "sensores");

            migrationBuilder.AlterColumn<string>(
                name: "name_varChar",
                table: "users",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "user_agent_text",
                table: "sessions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "payload_longtext",
                table: "sessions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "ip_address_varChar",
                table: "sessions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "sensores",
                keyColumn: "nombre_sensor_varChar",
                keyValue: null,
                column: "nombre_sensor_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "nombre_sensor_varChar",
                table: "sensores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "sensores",
                keyColumn: "modelo_varChar",
                keyValue: null,
                column: "modelo_varChar",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "modelo_varChar",
                table: "sensores",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "Permissionsid_int",
                table: "role_permission",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "slug_varChar",
                table: "permissions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name_varChar",
                table: "permissions",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "token_varChar",
                table: "password_reset_tokens",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "migration_varChar",
                table: "migrations",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "valor_digital_float",
                table: "mediciones",
                type: "int",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "queue_varChar",
                table: "jobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "payload_longtext",
                table: "jobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name_varChar",
                table: "job_batches",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "failed_jobs_ids_longtext",
                table: "job_batches",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "canceled_at_int",
                table: "job_batches",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "uuid_varChar",
                table: "failed_jobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "queue_text",
                table: "failed_jobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "payload_longtext",
                table: "failed_jobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "exception_longtext",
                table: "failed_jobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "connection_text",
                table: "failed_jobs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "owner_varChar",
                table: "cache_locks",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "value_mediumtext",
                table: "cache",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_role_permission_permissions_Permissionsid_int",
                table: "role_permission",
                column: "Permissionsid_int",
                principalTable: "permissions",
                principalColumn: "id_int");
        }
    }
}
