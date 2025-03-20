using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infrastructure.Migrations;

/// <inheritdoc />
public partial class v6_update_productfielddefinition : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "field_options",
            table: "product_field_definitions",
            type: "text",
            nullable: true,
            oldClrType: typeof(string),
            oldType: "text");

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

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "field_options",
            table: "product_field_definitions",
            type: "text",
            nullable: false,
            defaultValue: "",
            oldClrType: typeof(string),
            oldType: "text",
            oldNullable: true);

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
    }
}