using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_update_tbl_contact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "company_name",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "project_details",
                table: "contacts");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7148));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7155));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7157));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7159));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7160));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7161));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7163));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7164));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7166));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7167));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7168));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7169));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7171));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7172));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7173));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7175));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7176));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7179));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7180));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7181));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7183));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 17, 10, 28, 847, DateTimeKind.Utc).AddTicks(7184));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 20, 17, 10, 28, 849, DateTimeKind.Utc).AddTicks(719), "AQAAAAIAAYagAAAAECfdxiC1g2IcdWzugvxSLAGbpQkNHRTYA/b8wm3+3icbfgoOmkR7KYHN3ZVh3bc+HQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "company_name",
                table: "contacts",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "project_details",
                table: "contacts",
                type: "text",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(220));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(229));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(233));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(235));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(238));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(239));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(241));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(242));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(244));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(246));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(247));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(249));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(250));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(252));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(254));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(255));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(257));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(258));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(260));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(261));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(263));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(265));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 16, 15, 46, 806, DateTimeKind.Utc).AddTicks(267));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 20, 16, 15, 46, 808, DateTimeKind.Utc).AddTicks(3734), "AQAAAAIAAYagAAAAEEU0ahmFyarn4e1nycB+Uj7e5jaL905jDRlqF825e+PkF3CmT9VlYXbJTwc3tPNr6A==" });
        }
    }
}
