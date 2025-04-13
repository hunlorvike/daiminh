using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v3_remove_column_category_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_faqs_category_id",
                table: "faqs");

            migrationBuilder.DropColumn(
                name: "category_id",
                table: "faqs");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8926));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8931));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8933));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8934));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8935));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8936));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8938));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8940));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8941));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8942));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8943));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8945));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8946));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8947));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8948));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8949));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8951));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8952));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8954));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8955));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8957));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8958));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 13, 16, 36, 17, 811, DateTimeKind.Utc).AddTicks(8959));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 13, 16, 36, 17, 813, DateTimeKind.Utc).AddTicks(4354), "AQAAAAIAAYagAAAAEGJIbfSSg/Gq9BoC32yzcODIjftkVJQdw/fha+bF2dQlr9HxSS530r4R0Sa1+F08Gg==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "category_id",
                table: "faqs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3783));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3791));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3792));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3794));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3796));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3797));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3798));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3799));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3801));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3802));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3803));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3804));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3805));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3806));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3808));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3809));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3810));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3811));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3812));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3814));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3815));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3816));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 4, 12, 15, 40, 37, 281, DateTimeKind.Utc).AddTicks(3817));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 12, 15, 40, 37, 283, DateTimeKind.Utc).AddTicks(61), "AQAAAAIAAYagAAAAEC6jYz/8kZrlNjMFQF0f1BCeI/unScR0TRi5K3VCsXXcBgfJ6PtUEjJ2znI0r+HbIw==" });

            migrationBuilder.CreateIndex(
                name: "idx_faqs_category_id",
                table: "faqs",
                column: "category_id");
        }
    }
}
