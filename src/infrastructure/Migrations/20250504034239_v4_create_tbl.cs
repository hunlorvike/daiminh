using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v4_create_tbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanonicalUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoIndex = table.Column<bool>(type: "bit", nullable: false),
                    NoFollow = table.Column<bool>(type: "bit", nullable: false),
                    OgTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OgType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TwitterCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemaMarkup = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BreadcrumbJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SitemapPriority = table.Column<double>(type: "float", nullable: true),
                    SitemapChangeFrequency = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PopupModals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopupModals", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(301));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(304));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(305));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(306));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(308));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(309));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(310));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(311));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(312));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(314));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(315));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(316));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(317));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(318));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 999, DateTimeKind.Utc).AddTicks(319));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(9259));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(9262));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(9263));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(5771));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(5773));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(5775));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(5776));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(5777));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 994, DateTimeKind.Utc).AddTicks(5779));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8252));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8254));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8255));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8256));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8264));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8266));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8267));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8269));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8271));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8272));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8274));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8275));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8277));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 995, DateTimeKind.Utc).AddTicks(8278));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 36, 3, DateTimeKind.Utc).AddTicks(488));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 36, 3, DateTimeKind.Utc).AddTicks(491));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 36, 3, DateTimeKind.Utc).AddTicks(492));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7174));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7178));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7181));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7182));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7185));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7187));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7188));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7191));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7196));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7198));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7200));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7202));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7203));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7205));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7206));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7208));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 31,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7210));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 32,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7213));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 33,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 34,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7215));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 35,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 36,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7218));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 37,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 41,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 42,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 43,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7309));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 44,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7311));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 45,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7313));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 46,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7314));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 47,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7316));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 48,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7318));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 49,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7319));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 51,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7321));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 52,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7323));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 53,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7379));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 61,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7381));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 71,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7383));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 72,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7386));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 73,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(7387));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2184));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2186));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2192));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2193));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 996, DateTimeKind.Utc).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 998, DateTimeKind.Utc).AddTicks(4327));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 998, DateTimeKind.Utc).AddTicks(4329));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 3, 42, 35, 998, DateTimeKind.Utc).AddTicks(4331));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 5, 4, 3, 42, 35, 911, DateTimeKind.Utc).AddTicks(6021), "AQAAAAIAAYagAAAAEM/e5+RwX6sU/06vs8FwfQ5MoTS4rGiWUmOZFFDrIVe3Zowr3jGBMS566K2e/voXQw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "Pages");

            migrationBuilder.DropTable(
                name: "PopupModals");

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9675));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9678));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9679));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9680));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9682));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9684));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9687));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9688));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(9690));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(6855));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(6856));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(6858));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(6859));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(4133));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(4135));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(4137));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(4138));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(4139));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 716, DateTimeKind.Utc).AddTicks(4141));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3746));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3749));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3751));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3752));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3754));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3756));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3758));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3760));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3762));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3763));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3765));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3768));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3770));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(3771));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 723, DateTimeKind.Utc).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 723, DateTimeKind.Utc).AddTicks(2438));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 723, DateTimeKind.Utc).AddTicks(2440));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(869));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(872));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(873));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(875));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(877));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(878));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(880));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(881));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(883));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(885));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(887));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(888));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(890));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(891));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(893));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(894));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(896));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(897));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 31,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(899));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 32,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(901));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 33,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(902));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 34,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(904));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 35,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(905));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 36,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(907));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 37,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(909));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 41,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(911));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 42,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(913));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 43,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(914));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 44,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(916));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 45,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(917));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 46,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(919));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 47,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(920));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 48,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(922));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 49,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(924));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 51,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(925));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 52,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(927));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 53,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(974));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 61,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(975));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 71,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(977));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 72,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(978));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 73,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 718, DateTimeKind.Utc).AddTicks(980));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6836));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6837));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6839));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6840));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6841));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6843));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6844));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6845));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6846));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 717, DateTimeKind.Utc).AddTicks(6848));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(4773));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(4777));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 1, 4, 0, 38, 719, DateTimeKind.Utc).AddTicks(4778));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 5, 1, 4, 0, 38, 654, DateTimeKind.Utc).AddTicks(9936), "AQAAAAIAAYagAAAAEFeYD0uw8LEdqAcFeTWgnoW+r/l6/bTjPTz/BOtwPagJM40I2hMslIi25/OYZ/3G2A==" });
        }
    }
}
