using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_add_col_summary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "summary",
                table: "contents",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6177), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6178) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6181), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6181) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6182), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6182) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6183), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6183) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6184), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6185) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6186), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6186) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6187), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6187) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6188), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6188) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6189), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6189) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6190), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6191) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6192), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6192) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6193), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6193) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 13,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6194), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6195) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 14,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6196), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6196) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 15,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6197), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6197) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 16,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6198), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6198) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6199), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6200) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6201), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6201) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6202), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6202) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6203), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6203) });

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6204), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(6204) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5011), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5016) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5019), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5020) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5021), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5021) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5022), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5022) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5024), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5024) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5025), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5025) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5538), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5538) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5581), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5582) });

            migrationBuilder.UpdateData(
                table: "content_field_definitions",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5583), new DateTime(2025, 3, 23, 15, 24, 17, 514, DateTimeKind.Utc).AddTicks(5583) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7911), new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7912) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7927), new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7927) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7928), new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7928) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7929), new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7930) });

            migrationBuilder.UpdateData(
                table: "content_types",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7931), new DateTime(2025, 3, 23, 15, 24, 17, 513, DateTimeKind.Utc).AddTicks(7931) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8437), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8439) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8705), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8705) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8777), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8778) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8779), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8779) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8828), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8829) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8889), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8890) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8936), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8937) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8938), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(8938) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(9006), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(9006) });

            migrationBuilder.UpdateData(
                table: "product_field_definitions",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(9054), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(9054) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2406), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2407) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2410), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2410) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2411), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2411) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2412), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2412) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2413), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2413) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2414), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2414) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2415), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2415) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2416), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2416) });

            migrationBuilder.UpdateData(
                table: "product_types",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2417), new DateTime(2025, 3, 23, 15, 24, 17, 519, DateTimeKind.Utc).AddTicks(2417) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 510, DateTimeKind.Utc).AddTicks(8052), new DateTime(2025, 3, 23, 15, 24, 17, 510, DateTimeKind.Utc).AddTicks(8056) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 510, DateTimeKind.Utc).AddTicks(8059), new DateTime(2025, 3, 23, 15, 24, 17, 510, DateTimeKind.Utc).AddTicks(8059) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 510, DateTimeKind.Utc).AddTicks(8060), new DateTime(2025, 3, 23, 15, 24, 17, 510, DateTimeKind.Utc).AddTicks(8060) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9882), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9883) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9886), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9886) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9887), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9887) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9888), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9888) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9889), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9889) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9890), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9890) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 7,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9891), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9891) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 8,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9892), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9893) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 9,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9893), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9894) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 10,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9894), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9895) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 11,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9895), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9896) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 12,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9896), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9897) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 101,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9898), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9898) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 102,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9899), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9899) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 103,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9900), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9900) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 104,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9901), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9901) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 105,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9902), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9902) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 106,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9903), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9903) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 107,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9904), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9904) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 108,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9905), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9905) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 109,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9906), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9906) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 110,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9907), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9907) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 111,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9908), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9908) });

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "id",
                keyValue: 112,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9909), new DateTime(2025, 3, 23, 15, 24, 17, 511, DateTimeKind.Utc).AddTicks(9909) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 101, DateTimeKind.Utc).AddTicks(8972), "$2a$11$1bNa9qr.zQYwdbnRxu7vj.UHkfLWRgsGo.02GWngVvN8Nyw5sNgne", new DateTime(2025, 3, 23, 15, 24, 17, 101, DateTimeKind.Utc).AddTicks(8976) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 237, DateTimeKind.Utc).AddTicks(8120), "$2a$11$r7QPdi8zUGPJhY9xRUSxg.z2Dvgtx1rJ6GJHvO33ciDP1wNqU.Rve", new DateTime(2025, 3, 23, 15, 24, 17, 237, DateTimeKind.Utc).AddTicks(8127) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 23, 15, 24, 17, 373, DateTimeKind.Utc).AddTicks(9676), "$2a$11$ZtmKzqqAHGwJcxlBAsRqfuKNsiP.4KMlysg90ZaVykvEZRwMBKGSq", new DateTime(2025, 3, 23, 15, 24, 17, 373, DateTimeKind.Utc).AddTicks(9680) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "summary",
                table: "contents");

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
        }
    }
}
