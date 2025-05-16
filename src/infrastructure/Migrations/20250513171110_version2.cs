using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class version2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: true),
                    created_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    updated_by = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "user_permissions",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    permission_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_permissions", x => new { x.user_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_user_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_permissions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    role_id = table.Column<int>(type: "int", nullable: false),
                    permission_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => new { x.role_id, x.permission_id });
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_permission_id",
                        column: x => x.permission_id,
                        principalTable: "permissions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false),
                    role_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2721));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2723));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2724));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2725));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2726));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2728));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2729));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2730));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2731));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2732));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2733));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2734));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2735));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2737));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 488, DateTimeKind.Utc).AddTicks(2738));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 478, DateTimeKind.Utc).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 478, DateTimeKind.Utc).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 478, DateTimeKind.Utc).AddTicks(2347));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 478, DateTimeKind.Utc).AddTicks(2349));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 477, DateTimeKind.Utc).AddTicks(8742));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 477, DateTimeKind.Utc).AddTicks(8744));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 477, DateTimeKind.Utc).AddTicks(8746));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 477, DateTimeKind.Utc).AddTicks(8748));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 477, DateTimeKind.Utc).AddTicks(8750));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 477, DateTimeKind.Utc).AddTicks(8751));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1102));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1104));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1106));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1107));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1110));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1112));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1114));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1116));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1118));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1120));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1122));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1123));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1125));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(1126));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 491, DateTimeKind.Utc).AddTicks(8587));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 491, DateTimeKind.Utc).AddTicks(8589));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 491, DateTimeKind.Utc).AddTicks(8590));

            migrationBuilder.UpdateData(
                table: "pages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "published_at" },
                values: new object[] { new DateTime(2025, 5, 13, 17, 11, 7, 482, DateTimeKind.Utc).AddTicks(4162), new DateTime(2025, 5, 13, 17, 11, 7, 482, DateTimeKind.Utc).AddTicks(4158) });

            migrationBuilder.UpdateData(
                table: "pages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "created_at", "published_at" },
                values: new object[] { new DateTime(2025, 5, 13, 17, 11, 7, 482, DateTimeKind.Utc).AddTicks(4166), new DateTime(2025, 5, 13, 17, 11, 7, 482, DateTimeKind.Utc).AddTicks(4166) });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "Id", "created_at", "created_by", "name", "updated_at", "updated_by" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 13, 17, 11, 7, 476, DateTimeKind.Utc).AddTicks(9887), null, "Admin", null, null },
                    { 2, new DateTime(2025, 5, 13, 17, 11, 7, 476, DateTimeKind.Utc).AddTicks(9889), null, "Manager", null, null },
                    { 3, new DateTime(2025, 5, 13, 17, 11, 7, 476, DateTimeKind.Utc).AddTicks(9890), null, "User", null, null }
                });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8945));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8948));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8950));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8951));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8953));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8955));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8956));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8958));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8960));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8962));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8963));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8966));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8967));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8969));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8970));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8972));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8974));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8975));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 31,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8977));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 32,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8979));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 33,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8981));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 34,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8983));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 35,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8984));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 36,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(8986));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 37,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9035));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 41,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9037));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 42,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9039));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 43,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9040));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 44,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9042));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 45,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9043));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 46,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9045));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 47,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9046));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 48,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9048));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 49,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9049));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 51,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 52,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9053));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 53,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9108));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 61,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9109));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 71,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9114));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 72,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9116));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 73,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(9117));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4653));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4659));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4660));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4663));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4664));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4665));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4666));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4667));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4668));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 479, DateTimeKind.Utc).AddTicks(4670));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 481, DateTimeKind.Utc).AddTicks(4648));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 481, DateTimeKind.Utc).AddTicks(4651));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 13, 17, 11, 7, 481, DateTimeKind.Utc).AddTicks(4652));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 5, 13, 17, 11, 7, 411, DateTimeKind.Utc).AddTicks(4902), "AQAAAAIAAYagAAAAEI2vGsjl00w38KQG1VJucFul4zybSyTerF9mqkLMJpIxeE7kP1xzDfdoD2Av2INkSA==" });

            migrationBuilder.CreateIndex(
                name: "IX_permissions_name",
                table: "permissions",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_permission_id",
                table: "role_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_roles_name",
                table: "roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_permissions_permission_id",
                table: "user_permissions",
                column: "permission_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_role_id",
                table: "user_roles",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "user_permissions");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1096));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1148));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1149));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1151));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1152));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1154));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1155));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1158));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1162));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "attribute_values",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 914, DateTimeKind.Utc).AddTicks(1164));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7065));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7066));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "attributes",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(7069));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4839));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4843));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4844));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4845));

            migrationBuilder.UpdateData(
                table: "brands",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 907, DateTimeKind.Utc).AddTicks(4847));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3407));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3464));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3465));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3467));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3468));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3470));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3472));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3473));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3475));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3476));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3478));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3479));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3481));

            migrationBuilder.UpdateData(
                table: "categories",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(3482));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 917, DateTimeKind.Utc).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 917, DateTimeKind.Utc).AddTicks(1830));

            migrationBuilder.UpdateData(
                table: "faqs",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 917, DateTimeKind.Utc).AddTicks(1832));

            migrationBuilder.UpdateData(
                table: "pages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "published_at" },
                values: new object[] { new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9826), new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9823) });

            migrationBuilder.UpdateData(
                table: "pages",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "created_at", "published_at" },
                values: new object[] { new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9828), new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(9827) });

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(203));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(205));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(207));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(256));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(258));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(259));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(261));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(263));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(264));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(266));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(268));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(270));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(271));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(273));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(274));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(276));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(278));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(279));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 31,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(281));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 32,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(282));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 33,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(284));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 34,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(285));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 35,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(287));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 36,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(288));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 37,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(291));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 41,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(292));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 42,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(294));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 43,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(296));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 44,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(298));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 45,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(299));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 46,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(301));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 47,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(303));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 48,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(304));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 49,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(306));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 51,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(308));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 52,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(309));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 53,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(353));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 61,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(355));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 71,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(357));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 72,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(358));

            migrationBuilder.UpdateData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 73,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 909, DateTimeKind.Utc).AddTicks(360));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6313));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6314));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6316));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6317));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6318));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6319));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 7,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6321));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 8,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6322));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 9,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6323));

            migrationBuilder.UpdateData(
                table: "tags",
                keyColumn: "Id",
                keyValue: 10,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 908, DateTimeKind.Utc).AddTicks(6324));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(3717));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "testimonials",
                keyColumn: "Id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2025, 5, 4, 17, 45, 6, 910, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash" },
                values: new object[] { new DateTime(2025, 5, 4, 17, 45, 6, 846, DateTimeKind.Utc).AddTicks(9424), "AQAAAAIAAYagAAAAEG3pA2H6U75R+y88fVvNbKk4fXzsSLz7Jz537rOFNfCpMqrjUVVKJcgQdiol+lrJSg==" });
        }
    }
}
