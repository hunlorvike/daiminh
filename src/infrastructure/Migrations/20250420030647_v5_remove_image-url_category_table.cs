using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v5_remove_imageurl_category_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image_url",
                table: "categories");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6466));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6472));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6475));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6476));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6477));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6478));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6480));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6481));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6482));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6483));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6484));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6485));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6486));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6487));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6489));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6490));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6491));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6493));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6495));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6496));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6497));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 20, 3, 6, 45, 348, DateTimeKind.Utc).AddTicks(6498));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 20, 3, 6, 45, 349, DateTimeKind.Utc).AddTicks(9294), "AQAAAAIAAYagAAAAEK+mvXpebtHmlMwwzYvwlhig92egM4bfnz8ilTMy2OuYCTt8+MXODlBzcs2ZXecP4w==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "image_url",
                table: "categories",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6837));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6846));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6848));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6849));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6850));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6852));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6853));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6855));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6856));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6857));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6858));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6860));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6861));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6863));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6865));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6866));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6867));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6868));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6869));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6870));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6871));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 14, 16, 30, 45, 917, DateTimeKind.Utc).AddTicks(6873));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 14, 16, 30, 45, 919, DateTimeKind.Utc).AddTicks(3049), "AQAAAAIAAYagAAAAEIPPB4DZbKtG639PimiVIw5JpG28CNo357cLi9bs7+u3PFeX5RGxDaThtp/vh6RgeQ==" });
        }
    }
}
