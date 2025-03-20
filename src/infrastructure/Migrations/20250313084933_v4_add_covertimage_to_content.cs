using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations;

/// <inheritdoc />
public partial class v4_add_covertimage_to_content : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<string>(
            name: "cover_image_url",
            table: "contents",
            type: "character varying(500)",
            maxLength: 500,
            nullable: true);

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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "cover_image_url",
            table: "contents");

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 1,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 37, 43, 0, DateTimeKind.Utc).AddTicks(9629), new DateTime(2025, 3, 13, 8, 37, 43, 0, DateTimeKind.Utc).AddTicks(9632) });

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 2,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 37, 43, 0, DateTimeKind.Utc).AddTicks(9634), new DateTime(2025, 3, 13, 8, 37, 43, 0, DateTimeKind.Utc).AddTicks(9634) });

        migrationBuilder.UpdateData(
            table: "roles",
            keyColumn: "id",
            keyValue: 3,
            columns: new[] { "created_at", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 37, 43, 0, DateTimeKind.Utc).AddTicks(9635), new DateTime(2025, 3, 13, 8, 37, 43, 0, DateTimeKind.Utc).AddTicks(9635) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 1,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 37, 42, 707, DateTimeKind.Utc).AddTicks(6149), "$2a$11$dlfk1cYKAxK4IzUYPvm2QelN2maxzfTHjbLpFeU2wRJxPpPWexDni", new DateTime(2025, 3, 13, 8, 37, 42, 707, DateTimeKind.Utc).AddTicks(6151) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 2,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 37, 42, 804, DateTimeKind.Utc).AddTicks(2043), "$2a$11$X01uEddhmuzkEKZNJmqgmemVjZbAncvDfmbFgBBbjjrZ5GJbAZVfG", new DateTime(2025, 3, 13, 8, 37, 42, 804, DateTimeKind.Utc).AddTicks(2049) });

        migrationBuilder.UpdateData(
            table: "users",
            keyColumn: "id",
            keyValue: 3,
            columns: new[] { "created_at", "password_hash", "updated_at" },
            values: new object[] { new DateTime(2025, 3, 13, 8, 37, 42, 900, DateTimeKind.Utc).AddTicks(9036), "$2a$11$q/H1X.p5Phc44YMGpEjjM.W47OtJ6CQXwuIGaLT5IFaTzVmRBJtOC", new DateTime(2025, 3, 13, 8, 37, 42, 900, DateTimeKind.Utc).AddTicks(9041) });
    }
}