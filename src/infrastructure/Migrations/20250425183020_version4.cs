using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class version4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product_reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<int>(type: "int", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    user_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    rating = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", maxLength: 20, nullable: false, defaultValue: 0),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_reviews_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_reviews_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1894));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1900));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1902));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1903));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1904));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1906));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1907));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1908));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1909));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1911));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1912));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1913));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1914));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1916));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1917));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1919));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1923));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1924));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1925));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1926));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 30, 20, 295, DateTimeKind.Utc).AddTicks(1928));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 25, 18, 30, 20, 296, DateTimeKind.Utc).AddTicks(8384), "AQAAAAIAAYagAAAAEGO4C3dl4DR1mPBAE6pHMUIvW6xXnWiZqaZYV7B3XGbtPGu1bXqgryUiSgklNpliWA==" });

            migrationBuilder.CreateIndex(
                name: "idx_product_reviews_product_id",
                table: "product_reviews",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_reviews_rating",
                table: "product_reviews",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "idx_product_reviews_status",
                table: "product_reviews",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_product_reviews_user_id",
                table: "product_reviews",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_reviews");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7905));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7912));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7914));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7915));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7917));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7918));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7924));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7927));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7929));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7931));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7932));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7933));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7935));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7936));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7937));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7939));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7941));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7942));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7944));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7946));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7948));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7949));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 25, 18, 7, 24, 631, DateTimeKind.Utc).AddTicks(7950));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 25, 18, 7, 24, 633, DateTimeKind.Utc).AddTicks(2825), "AQAAAAIAAYagAAAAEEDGFYRf6oQPDz8e2EqJ987Fl8B5fVxQT7mTbUXrG4Z90n54vGqrbzPpG6AiqLXxxQ==" });
        }
    }
}
