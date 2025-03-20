using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Migrations;

/// <inheritdoc />
public partial class v5_create_file_and_folder : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "folders",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                parent_id = table.Column<int>(type: "integer", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_folders", x => x.id);
                table.ForeignKey(
                    name: "fk_folders_parent_id",
                    column: x => x.parent_id,
                    principalTable: "folders",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.CreateTable(
            name: "media_files",
            columns: table => new
            {
                id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                path = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                mime_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                size = table.Column<long>(type: "bigint", nullable: false),
                extension = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                folder_id = table.Column<int>(type: "integer", nullable: true),
                is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_media_files", x => x.id);
                table.ForeignKey(
                    name: "fk_media_files_folder_id",
                    column: x => x.folder_id,
                    principalTable: "folders",
                    principalColumn: "id",
                    onDelete: ReferentialAction.SetNull);
            });

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 1,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 14, 16, 24, 2, 908, DateTimeKind.Utc).AddTicks(9513), new DateTime(2025, 3, 14, 16, 24, 2, 908, DateTimeKind.Utc).AddTicks(9516) });

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 2,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 14, 16, 24, 2, 908, DateTimeKind.Utc).AddTicks(9518), new DateTime(2025, 3, 14, 16, 24, 2, 908, DateTimeKind.Utc).AddTicks(9518) });

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 3,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 14, 16, 24, 2, 908, DateTimeKind.Utc).AddTicks(9519), new DateTime(2025, 3, 14, 16, 24, 2, 908, DateTimeKind.Utc).AddTicks(9519) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 1,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 14, 16, 24, 2, 510, DateTimeKind.Utc).AddTicks(807), "$2a$11$zoGZEfAbQJASWgazRTxk1eWWzQusNmJABcBRe4UvOkjSI/IRmpMEK", new DateTime(2025, 3, 14, 16, 24, 2, 510, DateTimeKind.Utc).AddTicks(809) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 2,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 14, 16, 24, 2, 643, DateTimeKind.Utc).AddTicks(2869), "$2a$11$..o2Bhb83TUnEVHBsXPwfeFFJRpnYgB.rh8p4wmbgN95ogQjHUody", new DateTime(2025, 3, 14, 16, 24, 2, 643, DateTimeKind.Utc).AddTicks(2873) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 3,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 14, 16, 24, 2, 775, DateTimeKind.Utc).AddTicks(7323), "$2a$11$uRw/Gj8PFQ4I2EvRecXv/OLNc6vNuvZyGy.ZgpB7TYfsprTFKTe1u", new DateTime(2025, 3, 14, 16, 24, 2, 775, DateTimeKind.Utc).AddTicks(7328) });

        migrationBuilder.CreateIndex(
            name: "idx_folders_path",
            table: "folders",
            column: "path");

        migrationBuilder.CreateIndex(
            name: "IX_folders_parent_id",
            table: "folders",
            column: "parent_id");

        migrationBuilder.CreateIndex(
            name: "idx_media_files_path",
            table: "media_files",
            column: "path");

        migrationBuilder.CreateIndex(
            name: "IX_media_files_folder_id",
            table: "media_files",
            column: "folder_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "media_files");

        migrationBuilder.DropTable(
            name: "folders");

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 1,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 49, 33, 583, DateTimeKind.Utc).AddTicks(8374), new DateTime(2025, 3, 13, 8, 49, 33, 583, DateTimeKind.Utc).AddTicks(8377) });

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 2,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 49, 33, 583, DateTimeKind.Utc).AddTicks(8379), new DateTime(2025, 3, 13, 8, 49, 33, 583, DateTimeKind.Utc).AddTicks(8379) });

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 3,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 49, 33, 583, DateTimeKind.Utc).AddTicks(8380), new DateTime(2025, 3, 13, 8, 49, 33, 583, DateTimeKind.Utc).AddTicks(8380) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 1,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 49, 33, 289, DateTimeKind.Utc).AddTicks(4144), "$2a$11$l/A5viotCdI6k3Jw1qGCn.VY6k58FWsMIv4HfSVfSU4QzYD4N1FHm", new DateTime(2025, 3, 13, 8, 49, 33, 289, DateTimeKind.Utc).AddTicks(4146) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 2,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 49, 33, 386, DateTimeKind.Utc).AddTicks(4033), "$2a$11$Fxj.uSlh1eTprzNRVs0x4ulLu07LB2sFFx1wDhPwmLGncOKzTGHl2", new DateTime(2025, 3, 13, 8, 49, 33, 386, DateTimeKind.Utc).AddTicks(4038) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 3,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 49, 33, 483, DateTimeKind.Utc).AddTicks(7116), "$2a$11$hrnIO2jxbmp2OHoQ1mNDiObgpIceK7yj1FhmZ7uyKQDXgGzKxB6u2", new DateTime(2025, 3, 13, 8, 49, 33, 483, DateTimeKind.Utc).AddTicks(7120) });
    }
}