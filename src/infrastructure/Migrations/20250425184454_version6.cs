using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class version6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_article_products_order_index",
                table: "article_products");

            migrationBuilder.DropColumn(
                name: "order_index",
                table: "article_products");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5385));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5386));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5387));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5389));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5391));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5393));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5394));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5395));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5396));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5399));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5400));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5401));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5402));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5403));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5405));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5406));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 44, 53, 916, DateTimeKind.Utc).AddTicks(5407));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 25, 18, 44, 53, 918, DateTimeKind.Utc).AddTicks(875), "AQAAAAIAAYagAAAAED4LJMflFepa4lYCvCzNp6o6S4rQXGHKsoJDbM3FruMoUFnrg2Tn2CQ9nG47pryukw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order_index",
                table: "article_products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(382));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(388));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(390));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(392));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(393));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(395));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(396));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(397));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(399));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(402));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(406));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(407));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(408));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(416));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(417));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(418));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(420));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(421));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 40, 59, 397, DateTimeKind.Utc).AddTicks(422));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 25, 18, 40, 59, 398, DateTimeKind.Utc).AddTicks(5152), "AQAAAAIAAYagAAAAECUjSCf78YQc+ImmLPmlFpx6KPiEgBpJgP2SCDNTVLISRj2nYtg7aF2weOzJ1H4Q/g==" });

            migrationBuilder.CreateIndex(
                name: "idx_article_products_order_index",
                table: "article_products",
                column: "order_index");
        }
    }
}
