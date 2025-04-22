using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_remove_seo_for_brand : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "breadcrumb_json",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "canonical_url",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "meta_description",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "meta_keywords",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "meta_title",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "no_follow",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "no_index",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "og_description",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "og_image",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "og_title",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "og_type",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "schema_markup",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "sitemap_change_frequency",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "sitemap_priority",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "twitter_card",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "twitter_description",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "twitter_image",
                table: "brands");

            migrationBuilder.DropColumn(
                name: "twitter_title",
                table: "brands");

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4560));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4564));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4566));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4567));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4568));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4571));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4572));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4573));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4575));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4576));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4577));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4578));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4579));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4581));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4624));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4625));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4626));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4628));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4629));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4630));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4631));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 6, 7, 9, 515, DateTimeKind.Utc).AddTicks(4632));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 22, 6, 7, 9, 516, DateTimeKind.Utc).AddTicks(2669), "AQAAAAIAAYagAAAAEPRONAv6QRUwMD9yzLyk8oyPDGLYygsIRHxqyAiLg01ulPxXwgUviNxW9lLnaxkTGA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "breadcrumb_json",
                table: "brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "canonical_url",
                table: "brands",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "meta_description",
                table: "brands",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "meta_keywords",
                table: "brands",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "meta_title",
                table: "brands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "no_follow",
                table: "brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "no_index",
                table: "brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "og_description",
                table: "brands",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "og_image",
                table: "brands",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "og_title",
                table: "brands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "og_type",
                table: "brands",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "website");

            migrationBuilder.AddColumn<string>(
                name: "schema_markup",
                table: "brands",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "sitemap_change_frequency",
                table: "brands",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                defaultValue: "monthly");

            migrationBuilder.AddColumn<double>(
                name: "sitemap_priority",
                table: "brands",
                type: "float",
                nullable: true,
                defaultValue: 0.5);

            migrationBuilder.AddColumn<string>(
                name: "twitter_card",
                table: "brands",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                defaultValue: "summary_large_image");

            migrationBuilder.AddColumn<string>(
                name: "twitter_description",
                table: "brands",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "twitter_image",
                table: "brands",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "twitter_title",
                table: "brands",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8571));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8577));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8579));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8580));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8582));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8583));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8584));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8586));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8587));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8588));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8589));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8590));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8591));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8592));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8594));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8595));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8596));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8597));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8598));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8599));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8600));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8602));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 4, 22, 3, 21, 52, 28, DateTimeKind.Utc).AddTicks(8603));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 4, 22, 3, 21, 52, 29, DateTimeKind.Utc).AddTicks(5668), "AQAAAAIAAYagAAAAENue9AKlf7FyoGL44t1WYmfpOngiM2m/0V9PtdZz7dinW2lPKnXB62Wrz9EPYDxQkw==" });
        }
    }
}
