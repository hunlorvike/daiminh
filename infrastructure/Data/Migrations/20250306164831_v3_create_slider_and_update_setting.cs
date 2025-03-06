using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class v3_create_slider_and_update_setting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contents_users_UserId",
                table: "contents");

            migrationBuilder.DropIndex(
                name: "IX_contents_UserId",
                table: "contents");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "contents");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "users",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "tags",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "subscriber",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "settings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "group",
                table: "settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "settings",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "order_number",
                table: "settings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "roles",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "reviews",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "products",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_types",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_tags",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_image",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_field_values",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_field_definitions",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "product_categories",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "contents",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "content_types",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "content_tags",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "content_field_values",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "content_field_definitions",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "content_categories",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "contacts",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "comments",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "categories",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "is_active", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 6, 16, 48, 30, 758, DateTimeKind.Utc).AddTicks(5310), true, new DateTime(2025, 3, 6, 16, 48, 30, 758, DateTimeKind.Utc).AddTicks(5312) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "is_active", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 6, 16, 48, 30, 758, DateTimeKind.Utc).AddTicks(5315), true, new DateTime(2025, 3, 6, 16, 48, 30, 758, DateTimeKind.Utc).AddTicks(5321) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "is_active", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 6, 16, 48, 30, 758, DateTimeKind.Utc).AddTicks(5322), true, new DateTime(2025, 3, 6, 16, 48, 30, 758, DateTimeKind.Utc).AddTicks(5322) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "is_active", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 6, 16, 48, 30, 355, DateTimeKind.Utc).AddTicks(8477), true, "$2a$11$GUWN66GTfNCZnomIFtZSoeQ3fJCaH/BZ/hqjqp.ZaeCwlxfPGP2ri", new DateTime(2025, 3, 6, 16, 48, 30, 355, DateTimeKind.Utc).AddTicks(8480) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "is_active", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 6, 16, 48, 30, 492, DateTimeKind.Utc).AddTicks(8049), true, "$2a$11$N57JoO9zajTTlzsXUtxf3e12ENTbx7g.rHkr312psxMf2lnNqadu.", new DateTime(2025, 3, 6, 16, 48, 30, 492, DateTimeKind.Utc).AddTicks(8054) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "is_active", "password_hash", "role_id", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 6, 16, 48, 30, 624, DateTimeKind.Utc).AddTicks(8004), true, "$2a$11$r8FELTKWrCL1B7IKLQymxuzkJsoYF14b2cHZv.LgfNW7htnnb3UOu", 3, new DateTime(2025, 3, 6, 16, 48, 30, 624, DateTimeKind.Utc).AddTicks(8007) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_active",
                table: "users");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "subscriber");

            migrationBuilder.DropColumn(
                name: "description",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "group",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "order_number",
                table: "settings");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "roles");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "products");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_types");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_tags");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_image");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_field_values");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_field_definitions");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "product_categories");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "contents");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "content_types");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "content_tags");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "content_field_values");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "content_field_definitions");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "content_categories");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "categories");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "contents",
                type: "integer",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4042), new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4045) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4053), new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4053) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4054), new DateTime(2025, 2, 20, 14, 1, 39, 601, DateTimeKind.Utc).AddTicks(4055) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 189, DateTimeKind.Utc).AddTicks(3290), "$2a$11$LYaUUzKmSh626Ph5PIIp3.v37WK2eVknM9aD47D0ShBIyKAB15z7a", new DateTime(2025, 2, 20, 14, 1, 39, 189, DateTimeKind.Utc).AddTicks(3293) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 321, DateTimeKind.Utc).AddTicks(9446), "$2a$11$HqXoDet3MPqlb8SAprhAqeEscwagVhCe7GLjMx1RXy605oogl3ATS", new DateTime(2025, 2, 20, 14, 1, 39, 321, DateTimeKind.Utc).AddTicks(9451) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "role_id", "updated_at" },
                values: new object[] { new DateTime(2025, 2, 20, 14, 1, 39, 468, DateTimeKind.Utc).AddTicks(9425), "$2a$11$GPBhC/xKtZci0zwqry8rdOfpQf1qq2DCFlP8lO9Y7gO0YBv0gZcQ2", null, new DateTime(2025, 2, 20, 14, 1, 39, 468, DateTimeKind.Utc).AddTicks(9430) });

            migrationBuilder.CreateIndex(
                name: "IX_contents_UserId",
                table: "contents",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_contents_users_UserId",
                table: "contents",
                column: "UserId",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
