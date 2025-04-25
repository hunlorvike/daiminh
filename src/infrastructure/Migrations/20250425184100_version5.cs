using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class version5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_product_tags",
                table: "product_tags");

            migrationBuilder.DropIndex(
                name: "idx_product_tags_product_tag",
                table: "product_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_article_tags",
                table: "article_tags");

            migrationBuilder.DropIndex(
                name: "idx_article_tags_article_tag",
                table: "article_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_article_products",
                table: "article_products");

            migrationBuilder.DropIndex(
                name: "idx_article_products_article_product",
                table: "article_products");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "product_tags");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "product_tags");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "product_tags");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "product_tags");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "product_tags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "article_tags");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "article_tags");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "article_tags");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "article_tags");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "article_tags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "article_products");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "article_products");

            migrationBuilder.DropColumn(
                name: "created_by",
                table: "article_products");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "article_products");

            migrationBuilder.DropColumn(
                name: "updated_by",
                table: "article_products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_tags",
                table: "product_tags",
                columns: new[] { "product_id", "tag_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_article_tags",
                table: "article_tags",
                columns: new[] { "article_id", "tag_id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_article_products",
                table: "article_products",
                columns: new[] { "article_id", "product_id" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_product_tags",
                table: "product_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_article_tags",
                table: "article_tags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_article_products",
                table: "article_products");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "product_tags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "product_tags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "product_tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "product_tags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "product_tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "article_tags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "article_tags",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "article_tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "article_tags",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "article_tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "article_products",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "article_products",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<string>(
                name: "created_by",
                table: "article_products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "article_products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "updated_by",
                table: "article_products",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_product_tags",
                table: "product_tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_article_tags",
                table: "article_tags",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_article_products",
                table: "article_products",
                column: "Id");

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
                name: "idx_product_tags_product_tag",
                table: "product_tags",
                columns: new[] { "product_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_article_tags_article_tag",
                table: "article_tags",
                columns: new[] { "article_id", "tag_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_article_products_article_product",
                table: "article_products",
                columns: new[] { "article_id", "product_id" },
                unique: true);
        }
    }
}
