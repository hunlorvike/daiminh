using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v2_seeder_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "field_type",
                table: "product_field_definitions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "text",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldDefaultValue: "text");

            migrationBuilder.AlterColumn<string>(
                name: "field_type",
                table: "content_field_definitions",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "text",
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldDefaultValue: "text");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4042), new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4045) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4053), new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4053) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4054), new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4055) });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "created_at", "deleted_at", "email", "password_hash", "role_id", "updated_at", "username" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 2, 20, 14, 1, 39, 189, DateTimeKind.Utc).AddTicks(3290), null, "admin@admin.com", "$2a$11$LYaUUzKmSh626Ph5PIIp3.v37WK2eVknM9aD47D0ShBIyKAB15z7a", 1, new DateTime(2025, 2, 20, 14, 1, 39, 189, DateTimeKind.Utc).AddTicks(3293), "admin" },
                    { 2, new DateTime(2025, 2, 20, 14, 1, 39, 321, DateTimeKind.Utc).AddTicks(9446), null, "user@user.com", "$2a$11$HqXoDet3MPqlb8SAprhAqeEscwagVhCe7GLjMx1RXy605oogl3ATS", 2, new DateTime(2025, 2, 20, 14, 1, 39, 321, DateTimeKind.Utc).AddTicks(9451), "user" },
                    { 3, new DateTime(2025, 2, 20, 14, 1, 39, 468, DateTimeKind.Utc).AddTicks(9425), null, "manager@manager.com", "$2a$11$GPBhC/xKtZci0zwqry8rdOfpQf1qq2DCFlP8lO9Y7gO0YBv0gZcQ2", 3, new DateTime(2025, 2, 20, 14, 1, 39, 468, DateTimeKind.Utc).AddTicks(9430), "manager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "field_type",
                table: "product_field_definitions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "text",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "text");

            migrationBuilder.AlterColumn<string>(
                name: "field_type",
                table: "content_field_definitions",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "text",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "text");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 9, 18, 39, 29, 921, DateTimeKind.Utc).AddTicks(5634), new DateTime(2025, 2, 9, 18, 39, 29, 921, DateTimeKind.Utc).AddTicks(5637) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 9, 18, 39, 29, 921, DateTimeKind.Utc).AddTicks(5641), new DateTime(2025, 2, 9, 18, 39, 29, 921, DateTimeKind.Utc).AddTicks(5641) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 9, 18, 39, 29, 921, DateTimeKind.Utc).AddTicks(5643), new DateTime(2025, 2, 9, 18, 39, 29, 921, DateTimeKind.Utc).AddTicks(5643) });
        }
    }
}