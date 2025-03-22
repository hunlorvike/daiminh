using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v1_remove_unique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "idx_users_username",
                table: "users");

            migrationBuilder.DropIndex(
                name: "idx_settings_key",
                table: "settings");

            migrationBuilder.DropIndex(
                name: "idx_products_slug",
                table: "products");

            migrationBuilder.DropIndex(
                name: "IX_products_sku",
                table: "products");

            migrationBuilder.DropIndex(
                name: "idx_product_types_name",
                table: "product_types");

            migrationBuilder.DropIndex(
                name: "idx_product_types_slug",
                table: "product_types");

            migrationBuilder.DropIndex(
                name: "idx_categories_name",
                table: "categories");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6229), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6231) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6235), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6236) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6237), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6237) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6238), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6238) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6239), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6239) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6240), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6240) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6241), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6241) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6242), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6242) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6244), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6244) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6245), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6246) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6247), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6247) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6248), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6248) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6249), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6250) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6251), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6251) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6252), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6252) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6253), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6253) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6254), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6255) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6255), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6256) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6257), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6257) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6258), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6258) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6259), new DateTime(2025, 3, 22, 5, 6, 16, 135, DateTimeKind.Utc).AddTicks(6259) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6522), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6523) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6527), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6527) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6529), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6529) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6530), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6530) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6532), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6532) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6533), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(6533) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(7251), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(7251) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(7254), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(7254) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(7255), new DateTime(2025, 3, 22, 5, 6, 16, 138, DateTimeKind.Utc).AddTicks(7255) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9398), new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9399) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9410), new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9410) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9411), new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9412) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9413), new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9413) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9414), new DateTime(2025, 3, 22, 5, 6, 16, 137, DateTimeKind.Utc).AddTicks(9414) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(9249), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(9251) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(9854), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(9855) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(62), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(63) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(65), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(66) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(165), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(165) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(270), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(270) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(349), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(350) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(351), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(352) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(467), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(467) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(546), new DateTime(2025, 3, 22, 5, 6, 16, 145, DateTimeKind.Utc).AddTicks(546) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(422), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(426) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(428), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(429) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(429), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(430) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(430), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(431) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(431), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(431) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(432), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(432) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(433), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(433) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(434), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(434) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(435), new DateTime(2025, 3, 22, 5, 6, 16, 144, DateTimeKind.Utc).AddTicks(435) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 134, DateTimeKind.Utc).AddTicks(7257), new DateTime(2025, 3, 22, 5, 6, 16, 134, DateTimeKind.Utc).AddTicks(7260) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 134, DateTimeKind.Utc).AddTicks(7262), new DateTime(2025, 3, 22, 5, 6, 16, 134, DateTimeKind.Utc).AddTicks(7262) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 134, DateTimeKind.Utc).AddTicks(7263), new DateTime(2025, 3, 22, 5, 6, 16, 134, DateTimeKind.Utc).AddTicks(7263) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1037), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1039) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1042), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1042) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1043), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1044) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1045), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1045) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1046), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1046) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1048), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1048) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1049), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1049) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1051), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1051) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1052), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1052) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1053), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1053) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1054), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1055) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1056), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1056) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1057), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1057) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1058), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1059) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1060), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1060) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1061), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1061) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1062), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1063) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 106,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1064), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1064) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 107,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1065), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1066) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 108,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1066), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1067) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 109,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1068), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1068) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 110,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1069), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1069) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1070), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1071) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 112,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1072), new DateTime(2025, 3, 22, 5, 6, 16, 136, DateTimeKind.Utc).AddTicks(1072) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 15, 740, DateTimeKind.Utc).AddTicks(4830), "$2a$11$TSlJ8UxNplxu0g8fZXCp2ueucwJrDn.XfM35fP1mTW4byIXC8Qvha", new DateTime(2025, 3, 22, 5, 6, 15, 740, DateTimeKind.Utc).AddTicks(4832) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 15, 872, DateTimeKind.Utc).AddTicks(1765), "$2a$11$yui7.TLgRh/.vloZzFPQOe/V1sg4QLTI.KefW3PSDwYSRZQr3mMQG", new DateTime(2025, 3, 22, 5, 6, 15, 872, DateTimeKind.Utc).AddTicks(1770) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 22, 5, 6, 16, 2, DateTimeKind.Utc).AddTicks(9275), "$2a$11$pmOnYV6WiXclgWMiIGJDJuTD9p0Q97b14dLU/uNSSJ3EnJp2ba1G.", new DateTime(2025, 3, 22, 5, 6, 16, 2, DateTimeKind.Utc).AddTicks(9280) });

            migrationBuilder.CreateIndex(
                name: "idx_users_email",
                table: "users",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_users_username",
                table: "users",
                column: "username");

            migrationBuilder.CreateIndex(
                name: "idx_settings_key",
                table: "settings",
                column: "key");

            migrationBuilder.CreateIndex(
                name: "idx_products_sku",
                table: "products",
                column: "sku");

            migrationBuilder.CreateIndex(
                name: "idx_products_slug",
                table: "products",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "idx_product_types_name",
                table: "product_types",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "idx_product_types_slug",
                table: "product_types",
                column: "slug");

            migrationBuilder.CreateIndex(
                name: "idx_categories_name",
                table: "categories",
                column: "name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "idx_users_email",
                table: "users");

            migrationBuilder.DropIndex(
                name: "idx_users_username",
                table: "users");

            migrationBuilder.DropIndex(
                name: "idx_settings_key",
                table: "settings");

            migrationBuilder.DropIndex(
                name: "idx_products_sku",
                table: "products");

            migrationBuilder.DropIndex(
                name: "idx_products_slug",
                table: "products");

            migrationBuilder.DropIndex(
                name: "idx_product_types_name",
                table: "product_types");

            migrationBuilder.DropIndex(
                name: "idx_product_types_slug",
                table: "product_types");

            migrationBuilder.DropIndex(
                name: "idx_categories_name",
                table: "categories");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5876), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5876) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5878), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5878) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5879), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5879) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5880), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5880) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5881), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5881) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5882), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5882) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5883), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5884) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5884), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5885) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5886), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5886) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5887), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5888) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5889), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5889) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5890), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5890) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5891), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5891) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5892), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5893) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5893), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5894) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5895), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5895) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5896), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5896) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5897), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5897) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5898), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5898) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5899), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5899) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5900), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(5901) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5389), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5389) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5391), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5391) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5392), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5392) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5393), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5393) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5394), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5394) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5435), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(5435) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6019), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6019) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6021), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6021) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6022), new DateTime(2025, 3, 20, 10, 13, 43, 462, DateTimeKind.Utc).AddTicks(6023) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8892), new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8893) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8902), new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8902) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8903), new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8904) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8905), new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8905) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8906), new DateTime(2025, 3, 20, 10, 13, 43, 461, DateTimeKind.Utc).AddTicks(8906) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(4989), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(4989) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5666), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5667) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5801), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5802) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5804), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5804) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5888), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5889) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5986), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(5986) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6065), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6066) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6067), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6068) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6171), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6171) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6242), new DateTime(2025, 3, 20, 10, 13, 43, 469, DateTimeKind.Utc).AddTicks(6242) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7886), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7888) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7889), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7890) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7891), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7891) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7892), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7892) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7893), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7893) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7894), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7894) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7895), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7895) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7896), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7896) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7897), new DateTime(2025, 3, 20, 10, 13, 43, 468, DateTimeKind.Utc).AddTicks(7897) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7040), new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7041) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7042), new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7042) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7043), new DateTime(2025, 3, 20, 10, 13, 43, 458, DateTimeKind.Utc).AddTicks(7043) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9849), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9850) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9852), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9852) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9853), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9853) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9854), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9855) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9855), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9856) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9856), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9857) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9857), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9858) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9858), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9859) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9860), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9860) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9861), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9861) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9862), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9862) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9863), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9863) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9864), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9864) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9865), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9865) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9866), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9866) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9867), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9867) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9868), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9868) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 106,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9869), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9869) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 107,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9870), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9870) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 108,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9871), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9871) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 109,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9872), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9872) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 110,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9873), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9873) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9874), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9874) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 112,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9875), new DateTime(2025, 3, 20, 10, 13, 43, 459, DateTimeKind.Utc).AddTicks(9875) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 59, DateTimeKind.Utc).AddTicks(9884), "$2a$11$Z4lGO7u7lXj7PmulY2393u3QodblRNxqbPZ/UW0JiJAZQ59OgJbpO", new DateTime(2025, 3, 20, 10, 13, 43, 59, DateTimeKind.Utc).AddTicks(9886) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 178, DateTimeKind.Utc).AddTicks(3133), "$2a$11$F/ZE0HaFXlSvfGTsguS3d.2iaTfKjfEDG4y/xxBalhYWTJK9Mzdoy", new DateTime(2025, 3, 20, 10, 13, 43, 178, DateTimeKind.Utc).AddTicks(3136) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 10, 13, 43, 327, DateTimeKind.Utc).AddTicks(9359), "$2a$11$QvcQcI3zebftm./bAIJLPuVVs6823.MDSmRfND0k0HPqmyyAp9SV6", new DateTime(2025, 3, 20, 10, 13, 43, 327, DateTimeKind.Utc).AddTicks(9363) });

            migrationBuilder.CreateIndex(
                name: "idx_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_users_username",
                table: "users",
                column: "username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_settings_key",
                table: "settings",
                column: "key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_products_slug",
                table: "products",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_products_sku",
                table: "products",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_product_types_name",
                table: "product_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_product_types_slug",
                table: "product_types",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_categories_name",
                table: "categories",
                column: "name",
                unique: true);
        }
    }
}
