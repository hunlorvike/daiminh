using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations;

/// <inheritdoc />
public partial class v3_update_category : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "breadcrumb_json",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "canonical_url",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "meta_description",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "meta_keywords",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "meta_title",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "no_follow",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "no_index",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "og_description",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "og_image",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "og_title",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "og_type",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "schema_markup",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "sitemap_change_frequency",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "sitemap_priority",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "twitter_card",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "twitter_description",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "twitter_image",
            table: "categories");

        migrationBuilder.DropColumn(
            name: "twitter_title",
            table: "categories");

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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "breadcrumb_json",
            table: "categories",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "canonical_url",
            table: "categories",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "meta_description",
            table: "categories",
            type: "nvarchar(300)",
            maxLength: 300,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "meta_keywords",
            table: "categories",
            type: "nvarchar(200)",
            maxLength: 200,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "meta_title",
            table: "categories",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<bool>(
            name: "no_follow",
            table: "categories",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<bool>(
            name: "no_index",
            table: "categories",
            type: "bit",
            nullable: false,
            defaultValue: false);

        migrationBuilder.AddColumn<string>(
            name: "og_description",
            table: "categories",
            type: "nvarchar(300)",
            maxLength: 300,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "og_image",
            table: "categories",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "og_title",
            table: "categories",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "og_type",
            table: "categories",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            defaultValue: "website");

        migrationBuilder.AddColumn<string>(
            name: "schema_markup",
            table: "categories",
            type: "nvarchar(max)",
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "sitemap_change_frequency",
            table: "categories",
            type: "nvarchar(20)",
            maxLength: 20,
            nullable: true,
            defaultValue: "monthly");

        migrationBuilder.AddColumn<double>(
            name: "sitemap_priority",
            table: "categories",
            type: "float",
            nullable: true,
            defaultValue: 0.5);

        migrationBuilder.AddColumn<string>(
            name: "twitter_card",
            table: "categories",
            type: "nvarchar(50)",
            maxLength: 50,
            nullable: true,
            defaultValue: "summary_large_image");

        migrationBuilder.AddColumn<string>(
            name: "twitter_description",
            table: "categories",
            type: "nvarchar(300)",
            maxLength: 300,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "twitter_image",
            table: "categories",
            type: "nvarchar(255)",
            maxLength: 255,
            nullable: true);

        migrationBuilder.AddColumn<string>(
            name: "twitter_title",
            table: "categories",
            type: "nvarchar(100)",
            maxLength: 100,
            nullable: true);

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 1,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8689));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 2,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8691));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 3,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8692));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 4,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8693));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 5,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8694));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 6,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8695));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 7,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8696));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 8,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8697));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 9,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8698));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 10,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8699));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 11,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8700));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 12,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8701));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 13,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8702));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 14,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8703));

        migrationBuilder.UpdateData(
            table: "attribute_values",
            keyColumn: "Id",
            keyValue: 15,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(8704));

        migrationBuilder.UpdateData(
            table: "attributes",
            keyColumn: "Id",
            keyValue: 1,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(4607));

        migrationBuilder.UpdateData(
            table: "attributes",
            keyColumn: "Id",
            keyValue: 2,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(4608));

        migrationBuilder.UpdateData(
            table: "attributes",
            keyColumn: "Id",
            keyValue: 3,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(4610));

        migrationBuilder.UpdateData(
            table: "attributes",
            keyColumn: "Id",
            keyValue: 4,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(4611));

        migrationBuilder.UpdateData(
            table: "brands",
            keyColumn: "Id",
            keyValue: 1,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(2704));

        migrationBuilder.UpdateData(
            table: "brands",
            keyColumn: "Id",
            keyValue: 2,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(2706));

        migrationBuilder.UpdateData(
            table: "brands",
            keyColumn: "Id",
            keyValue: 3,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(2707));

        migrationBuilder.UpdateData(
            table: "brands",
            keyColumn: "Id",
            keyValue: 4,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(2708));

        migrationBuilder.UpdateData(
            table: "brands",
            keyColumn: "Id",
            keyValue: 5,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(2709));

        migrationBuilder.UpdateData(
            table: "brands",
            keyColumn: "Id",
            keyValue: 6,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(2710));

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9196), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 2,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9198), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 3,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9199), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 4,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9201), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 5,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9202), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 6,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9204), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 7,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9206), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 8,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9237), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 9,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9239), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 10,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9241), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 11,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9242), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 12,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9244), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 13,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9245), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "categories",
            keyColumn: "Id",
            keyValue: 14,
            columns: new[] { "breadcrumb_json", "canonical_url", "created_at", "meta_description", "meta_keywords", "meta_title", "og_description", "og_image", "og_title", "og_type", "schema_markup", "sitemap_change_frequency", "sitemap_priority", "twitter_card", "twitter_description", "twitter_image", "twitter_title" },
            values: new object[] { null, null, new DateTime(2025, 4, 29, 3, 44, 48, 846, DateTimeKind.Utc).AddTicks(9247), null, null, null, null, null, null, "website", null, "monthly", 0.5, "summary_large_image", null, null, null });

        migrationBuilder.UpdateData(
            table: "faqs",
            keyColumn: "Id",
            keyValue: 1,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 849, DateTimeKind.Utc).AddTicks(940));

        migrationBuilder.UpdateData(
            table: "faqs",
            keyColumn: "Id",
            keyValue: 2,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 849, DateTimeKind.Utc).AddTicks(942));

        migrationBuilder.UpdateData(
            table: "faqs",
            keyColumn: "Id",
            keyValue: 3,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 849, DateTimeKind.Utc).AddTicks(944));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 1,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1937));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 2,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1964));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 3,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1966));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 4,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1967));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 5,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1969));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 6,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1970));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 11,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1972));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 12,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1973));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 13,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1975));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 14,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1976));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 15,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1977));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 16,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1979));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 17,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1980));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 18,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1982));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 21,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1983));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 22,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1985));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 23,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1987));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 24,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1988));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 31,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1989));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 32,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1991));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 33,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1992));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 34,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1993));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 35,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1995));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 36,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1996));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 37,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1997));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 41,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(1998));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 42,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2000));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 43,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2001));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 44,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2002));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 45,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2004));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 46,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2006));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 47,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2007));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 48,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2008));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 49,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2009));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 51,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2011));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 52,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2012));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 53,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2057));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 61,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2058));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 71,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2059));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 72,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2060));

        migrationBuilder.UpdateData(
            table: "settings",
            keyColumn: "Id",
            keyValue: 73,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(2062));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 1,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(514));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 2,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(515));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 3,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(516));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 4,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(517));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 5,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(519));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 6,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(520));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 7,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(521));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 8,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(522));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 9,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(523));

        migrationBuilder.UpdateData(
            table: "tags",
            keyColumn: "Id",
            keyValue: 10,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(524));

        migrationBuilder.UpdateData(
            table: "testimonials",
            keyColumn: "Id",
            keyValue: 1,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(6617));

        migrationBuilder.UpdateData(
            table: "testimonials",
            keyColumn: "Id",
            keyValue: 2,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(6619));

        migrationBuilder.UpdateData(
            table: "testimonials",
            keyColumn: "Id",
            keyValue: 3,
            column: "created_at",
            value: new DateTime(2025, 4, 29, 3, 44, 48, 847, DateTimeKind.Utc).AddTicks(6621));

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "Id",
            keyValue: 1,
            columns: new[] { "created_at", "password_hash" },
            values: new object[] { new DateTime(2025, 4, 29, 3, 44, 48, 810, DateTimeKind.Utc).AddTicks(4414), "AQAAAAIAAYagAAAAEEGsXko+r8CaeyC70ka+OrGAqCCqUmANz1mZTquSWeno7zFtEFVJXDAU/ZNeU43wLg==" });
    }
}
