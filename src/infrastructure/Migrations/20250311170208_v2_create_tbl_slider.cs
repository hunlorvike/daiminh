using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_create_tbl_slider : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sliders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    LinkUrl = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    OverlayHtml = table.Column<string>(type: "text", nullable: true),
                    OverlayPosition = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sliders", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sliders");

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 9, 17, 49, 15, 378, DateTimeKind.Utc).AddTicks(1610), new DateTime(2025, 3, 9, 17, 49, 15, 378, DateTimeKind.Utc).AddTicks(1613) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 9, 17, 49, 15, 378, DateTimeKind.Utc).AddTicks(1615), new DateTime(2025, 3, 9, 17, 49, 15, 378, DateTimeKind.Utc).AddTicks(1620) });

            migrationBuilder.UpdateData(
                table: "roles",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 9, 17, 49, 15, 378, DateTimeKind.Utc).AddTicks(1621), new DateTime(2025, 3, 9, 17, 49, 15, 378, DateTimeKind.Utc).AddTicks(1621) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 9, 17, 49, 14, 990, DateTimeKind.Utc).AddTicks(5012), "$2a$11$xPyAqsgv8RCO72cbRragQuyskW0yuPgorfpl4Nrw1.MHU3eT5YlXW", new DateTime(2025, 3, 9, 17, 49, 14, 990, DateTimeKind.Utc).AddTicks(5085) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 9, 17, 49, 15, 119, DateTimeKind.Utc).AddTicks(8474), "$2a$11$v8qL4XQTpJKKh.zvXbBT8.ty3k0rkMpxR.Yt9aUQPobuPz1WSEtYa", new DateTime(2025, 3, 9, 17, 49, 15, 119, DateTimeKind.Utc).AddTicks(8477) });

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "password_hash", "updated_at" },
                values: new object[] { new DateTime(2025, 3, 9, 17, 49, 15, 250, DateTimeKind.Utc).AddTicks(3574), "$2a$11$6Nr2y/BvIyMIr.YGeZ.c5etdvjqOYi4uBbHoX4PH89Nhn0y.JTc/6", new DateTime(2025, 3, 9, 17, 49, 15, 250, DateTimeKind.Utc).AddTicks(3575) });
        }
    }
}