using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_update_tbl_setting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "settings",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "text",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3783), "text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3791), "url" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3792), "email" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3794), "text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3796), "textarea" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3797), "phone" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3798), "email" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3799), "html" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3801), "text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3802), "textarea" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3803), "image" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3804), "url" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3805), "url" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3806), "url" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3808), "url" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3809), "url" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3810), "text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3811), "number" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3812), "text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3814), "text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3815), "boolean" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3816), "text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3817), "email" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 283, DateTimeKind.Utc).AddTicks(61), "AQAAAAIAAYagAAAAEC6jYz/8kZrlNjMFQF0f1BCeI/unScR0TRi5K3VCsXXcBgfJ6PtUEjJ2znI0r+HbIw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "settings",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "text");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9873), "Text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9879), "URL" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9881), "Email" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9882), "Text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9884), "TextArea" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9885), "Phone" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9887), "Email" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9888), "HTML" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9890), "Text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9891), "TextArea" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9893), "Image" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9894), "URL" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9895), "URL" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9896), "URL" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9898), "URL" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9899), "URL" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9900), "Text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9901), "Number" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9903), "Text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9904), "Password" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9905), "Boolean" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9906), "Text" });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "CreatedAt", "type" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 444, DateTimeKind.Utc).AddTicks(9908), "Email" });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 23, 0, 446, DateTimeKind.Utc).AddTicks(5851), "AQAAAAIAAYagAAAAEFmzwDq6IEqZQvuhIGjz+lOnpAjgwuYH+RZ2Vu7yrM9poX06bGhdT2oEkxkmKXOamw==" });
        }
    }
}
