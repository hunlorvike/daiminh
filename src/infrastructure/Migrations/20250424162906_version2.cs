using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class version2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_variants");

            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "articles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8824));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8832));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8834));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8835));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8836));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8839));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8840));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8841));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8843));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8844));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8845));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8846));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8847));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8849));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8850));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8851));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8852));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8854));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8855));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8856));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8857));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8858));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 24, 16, 29, 3, 829, DateTimeKind.Utc).AddTicks(8860));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 24, 16, 29, 3, 831, DateTimeKind.Utc).AddTicks(4057), "AQAAAAIAAYagAAAAENKQtAh9VkAfJyXVEnM2lpN5ikqLg4DQsGXHS8gtt4EYvZzwLEfXHtRM1mAQyAbxWw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "articles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "product_variants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_id = table.Column<int>(type: "int", nullable: false),
                    color = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    image_url = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    is_active = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    packaging = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    sku = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    stock_quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_product_variants_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5364));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5369));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5371));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5372));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5374));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5377));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5378));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5381));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5384));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5386));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5387));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5388));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5390));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5391));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5392));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5393));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5395));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5397));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 15, 23, 52, 110, DateTimeKind.Utc).AddTicks(5398));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 22, 15, 23, 52, 111, DateTimeKind.Utc).AddTicks(8075), "AQAAAAIAAYagAAAAEBfGdK3hIIfSblPbhjJQS0oSXW3MVf0xhHVcBN0Th3XljAVR+sypTRKQuLKe5cwOSw==" });

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_is_active",
                table: "product_variants",
                column: "is_active");

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_product_id",
                table: "product_variants",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_product_variants_sku",
                table: "product_variants",
                column: "sku",
                unique: true);
        }
    }
}
