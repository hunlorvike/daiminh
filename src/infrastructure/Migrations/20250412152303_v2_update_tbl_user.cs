using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_update_tbl_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "users",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "projects",
                type: "double precision",
                nullable: true,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "projects",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "products",
                type: "double precision",
                nullable: true,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "products",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "galleries",
                type: "double precision",
                nullable: true,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "galleries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "categories",
                type: "double precision",
                nullable: true,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "categories",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "brands",
                type: "double precision",
                nullable: true,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "brands",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "articles",
                type: "double precision",
                nullable: true,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "articles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "monthly");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9895));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9899));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9903));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9904));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9905));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9906));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9908));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "full_name", "is_active", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 446, DateTimeKind.Utc).AddTicks(5851), "Quản trị viên", true, "AQAAAAIAAYagAAAAEFmzwDq6IEqZQvuhIGjz+lOnpAjgwuYH+RZ2Vu7yrM9poX06bGhdT2oEkxkmKXOamw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "users");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "projects",
                type: "double precision",
                nullable: false,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "projects",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "products",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "galleries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "galleries",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "categories",
                type: "double precision",
                nullable: false,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "categories",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "brands",
                type: "double precision",
                nullable: false,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "brands",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "monthly");

            migrationBuilder.AlterColumn<double>(
                name: "sitemap_priority",
                table: "articles",
                type: "double precision",
                nullable: false,
                defaultValue: 0.5,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true,
                oldDefaultValue: 0.5);

            migrationBuilder.AlterColumn<string>(
                name: "sitemap_change_frequency",
                table: "articles",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "monthly",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true,
                oldDefaultValue: "monthly");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6386));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6390));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6392));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6393));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6395));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6400));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6401));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6402));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6403));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6405));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6406));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6407));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6408));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6409));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6411));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6412));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6413));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6414));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6416));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6417));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6418));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 8, 18, 31, 27, 464, DateTimeKind.Utc).AddTicks(6420));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 8, 18, 31, 27, 465, DateTimeKind.Utc).AddTicks(9869), "AQAAAAIAAYagAAAAEHxXe/1Yo3vxdfzGy68uiYvJT+lPKIk76Y9VPWRburWUlZ3j3MrA991A60jplCpkqg==" });
        }
    }
}
