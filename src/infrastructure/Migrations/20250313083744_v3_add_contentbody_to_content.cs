using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v3_add_contentbody_to_content : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sliders",
                table: "Sliders");

            migrationBuilder.RenameTable(
                name: "Sliders",
                newName: "slider");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "slider",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "slider",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "slider",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "OverlayPosition",
                table: "slider",
                newName: "overlay_position");

            migrationBuilder.RenameColumn(
                name: "OverlayHtml",
                table: "slider",
                newName: "overlay_html");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "slider",
                newName: "order_number");

            migrationBuilder.RenameColumn(
                name: "LinkUrl",
                table: "slider",
                newName: "link_url");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "slider",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "slider",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "slider",
                newName: "deleted_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "slider",
                newName: "created_at");

            migrationBuilder.AddColumn<string>(
                name: "content_body",
                table: "contents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "slider",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "overlay_position",
                table: "slider",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "slider",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddPrimaryKey(
                name: "PK_slider",
                table: "slider",
                column: "id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_slider",
                table: "slider");

            migrationBuilder.DropColumn(
                name: "content_body",
                table: "contents");

            migrationBuilder.RenameTable(
                name: "slider",
                newName: "Sliders");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "Sliders",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Sliders",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Sliders",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "overlay_position",
                table: "Sliders",
                newName: "OverlayPosition");

            migrationBuilder.RenameColumn(
                name: "overlay_html",
                table: "Sliders",
                newName: "OverlayHtml");

            migrationBuilder.RenameColumn(
                name: "order_number",
                table: "Sliders",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "link_url",
                table: "Sliders",
                newName: "LinkUrl");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Sliders",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "Sliders",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "deleted_at",
                table: "Sliders",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Sliders",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sliders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<int>(
                name: "OverlayPosition",
                table: "Sliders",
                type: "integer",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Sliders",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sliders",
                table: "Sliders",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 11, 17, 2, 7, 939, DateTimeKind.Utc).AddTicks(3936), new DateTime(2025, 3, 11, 17, 2, 7, 939, DateTimeKind.Utc).AddTicks(3941) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 11, 17, 2, 7, 939, DateTimeKind.Utc).AddTicks(3946), new DateTime(2025, 3, 11, 17, 2, 7, 939, DateTimeKind.Utc).AddTicks(3952) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 11, 17, 2, 7, 939, DateTimeKind.Utc).AddTicks(3953), new DateTime(2025, 3, 11, 17, 2, 7, 939, DateTimeKind.Utc).AddTicks(3953) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 11, 17, 2, 7, 383, DateTimeKind.Utc).AddTicks(7167), "$2a$11$SZ63DY7WFuGU5aMGt8a2x.db5Nr0eieJ4AC.VWGZXMhVGo8QITjNi", new DateTime(2025, 3, 11, 17, 2, 7, 383, DateTimeKind.Utc).AddTicks(7172) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 11, 17, 2, 7, 551, DateTimeKind.Utc).AddTicks(3523), "$2a$11$FnLuVzGJeTeD63uLSCRvBu.sVDrqJ1KKoAXpHyvb3DTkUuPwex3iu", new DateTime(2025, 3, 11, 17, 2, 7, 551, DateTimeKind.Utc).AddTicks(3530) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 11, 17, 2, 7, 738, DateTimeKind.Utc).AddTicks(4254), "$2a$11$DGUFurlf9PoGdh6VOfvk4.vQ4qDDCsecClos9pgkTGnVIHG3/b7LK", new DateTime(2025, 3, 11, 17, 2, 7, 738, DateTimeKind.Utc).AddTicks(4261) });
        }
    }
}
