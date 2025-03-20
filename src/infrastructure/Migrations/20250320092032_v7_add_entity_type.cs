using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v7_add_entity_type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "entity_type",
                table: "tags",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "entity_type",
                table: "categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 9, 20, 30, 451, DateTimeKind.Utc).AddTicks(909), new DateTime(2025, 3, 20, 9, 20, 30, 451, DateTimeKind.Utc).AddTicks(911) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 9, 20, 30, 451, DateTimeKind.Utc).AddTicks(915), new DateTime(2025, 3, 20, 9, 20, 30, 451, DateTimeKind.Utc).AddTicks(915) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 9, 20, 30, 451, DateTimeKind.Utc).AddTicks(916), new DateTime(2025, 3, 20, 9, 20, 30, 451, DateTimeKind.Utc).AddTicks(916) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 9, 20, 30, 161, DateTimeKind.Utc).AddTicks(6747), "$2a$11$YgY6faBsD.RNYjFzEuHKU.YP2wKxYxIF/Fvs2vqEz7HU5D2zTW2FW", new DateTime(2025, 3, 20, 9, 20, 30, 161, DateTimeKind.Utc).AddTicks(6749) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 9, 20, 30, 258, DateTimeKind.Utc).AddTicks(1113), "$2a$11$smxqqJCFMuW5TYtAba6OluGHZKiYzIzTBY2WuwDMnMrMpll0Ky76y", new DateTime(2025, 3, 20, 9, 20, 30, 258, DateTimeKind.Utc).AddTicks(1117) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 20, 9, 20, 30, 355, DateTimeKind.Utc).AddTicks(5366), "$2a$11$fKgbvFVB0ZjqOaXv53DumOSmz48GsrzK0PnUH3uC3xmFnFwlX32xi", new DateTime(2025, 3, 20, 9, 20, 30, 355, DateTimeKind.Utc).AddTicks(5369) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "entity_type",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "entity_type",
                table: "categories");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 19, 10, 23, 50, 34, DateTimeKind.Utc).AddTicks(4654), new DateTime(2025, 3, 19, 10, 23, 50, 34, DateTimeKind.Utc).AddTicks(4655) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 19, 10, 23, 50, 34, DateTimeKind.Utc).AddTicks(4729), new DateTime(2025, 3, 19, 10, 23, 50, 34, DateTimeKind.Utc).AddTicks(4729) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 19, 10, 23, 50, 34, DateTimeKind.Utc).AddTicks(4731), new DateTime(2025, 3, 19, 10, 23, 50, 34, DateTimeKind.Utc).AddTicks(4731) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 19, 10, 23, 49, 676, DateTimeKind.Utc).AddTicks(7115), "$2a$11$LyJo4Jq1YJhopzg/ZEFKXed.Fkc6sgSqsHfaiIpAB1am9FKV9.nZm", new DateTime(2025, 3, 19, 10, 23, 49, 676, DateTimeKind.Utc).AddTicks(7118) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 19, 10, 23, 49, 792, DateTimeKind.Utc).AddTicks(381), "$2a$11$zG/Zau5Q1d1G9CEuIgsHJ.ZrdrbhDck0.lUIMhF4QpNI8JRHYu1qG", new DateTime(2025, 3, 19, 10, 23, 49, 792, DateTimeKind.Utc).AddTicks(382) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 19, 10, 23, 49, 909, DateTimeKind.Utc).AddTicks(1591), "$2a$11$vv60AIqO99oOAVmsmndBL./3WVUFQmVe3i5lfmsQCHwrYtwVsOJ9m", new DateTime(2025, 3, 19, 10, 23, 49, 909, DateTimeKind.Utc).AddTicks(1595) });
        }
    }
}
