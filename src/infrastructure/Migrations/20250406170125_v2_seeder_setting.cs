using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v2_seeder_setting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "settings",
                columns: new[] { "Id", "category", "CreatedAt", "CreatedBy", "default_value", "description", "is_active", "key", "type", "UpdatedAt", "UpdatedBy", "value" },
                values: new object[,]
                {
                    { 1, "General", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9462), null, "Đại Minh Việt Nam", "Tên website hiển thị trên trang và tiêu đề trình duyệt.", true, "SiteName", "Text", null, null, "Đại Minh Việt Nam" },
                    { 2, "General", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9468), null, "https://localhost:7001", "Địa chỉ URL chính của website (ví dụ: https://www.example.com).", true, "SiteUrl", "URL", null, null, "https://localhost:7001" },
                    { 3, "General", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9469), null, "sondaiminh@gmail.com", "Địa chỉ email quản trị viên để nhận thông báo hệ thống.", true, "AdminEmail", "Email", null, null, "sondaiminh@gmail.com" },
                    { 5, "Contact", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9471), null, "Đại Minh Việt Nam", "Tên công ty hoặc tổ chức sở hữu website.", true, "CompanyName", "Text", null, null, "Đại Minh Việt Nam" },
                    { 6, "Contact", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9472), null, "123 Main Street, Anytown, CA 91234", "Địa chỉ liên hệ đầy đủ.", true, "ContactAddress", "TextArea", null, null, "123 Main Street, Anytown, CA 91234" },
                    { 7, "Contact", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9473), null, "(123) 456-7890", "Số điện thoại liên hệ chính.", true, "ContactPhone", "Phone", null, null, "(123) 456-7890" },
                    { 8, "Contact", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9474), null, "contact@example.com", "Địa chỉ email hiển thị công khai để liên hệ.", true, "ContactEmail", "Email", null, null, "contact@example.com" },
                    { 9, "Contact", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9476), null, null, "Mã nhúng HTML của bản đồ (ví dụ: Google Maps iframe).", true, "ContactMapEmbed", "HTML", null, null, null },
                    { 10, "SEO", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9477), null, "Welcome to My Application", "Tiêu đề meta mặc định cho các trang không có tiêu đề riêng.", true, "DefaultMetaTitle", "Text", null, null, "Welcome to My Application" },
                    { 11, "SEO", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9478), null, "This is the default description for My Application.", "Mô tả meta mặc định (dưới 160 ký tự).", true, "DefaultMetaDescription", "TextArea", null, null, "This is the default description for My Application." },
                    { 12, "SEO", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9479), null, "/image/icon.jpg", "Đường dẫn đến file favicon.ico hoặc ảnh favicon.", true, "FaviconUrl", "Image", null, null, "/image/icon.jpg" },
                    { 13, "Social Media", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9481), null, null, "URL trang Facebook.", true, "SocialFacebookUrl", "URL", null, null, null },
                    { 14, "Social Media", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9482), null, null, "URL trang Twitter (X).", true, "SocialTwitterUrl", "URL", null, null, null },
                    { 15, "Social Media", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9483), null, null, "URL trang Instagram.", true, "SocialInstagramUrl", "URL", null, null, null },
                    { 16, "Social Media", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9484), null, null, "URL trang LinkedIn.", true, "SocialLinkedInUrl", "URL", null, null, null },
                    { 17, "Social Media", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9486), null, null, "URL kênh Youtube.", true, "SocialYoutubeUrl", "URL", null, null, null },
                    { 18, "Email", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9487), null, "smtp.example.com", "Địa chỉ máy chủ SMTP.", true, "SmtpHost", "Text", null, null, "smtp.example.com" },
                    { 19, "Email", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9488), null, "587", "Cổng SMTP (ví dụ: 587, 465, 25).", true, "SmtpPort", "Number", null, null, "587" },
                    { 20, "Email", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9489), null, "user@example.com", "Tên đăng nhập SMTP.", true, "SmtpUsername", "Text", null, null, "user@example.com" },
                    { 21, "Email", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9490), null, null, "**QUAN TRỌNG**: Mật khẩu SMTP. Nên cấu hình qua UI, không seed giá trị thật.", true, "SmtpPassword", "Password", null, null, null },
                    { 22, "Email", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9492), null, "true", "Sử dụng mã hóa SSL/TLS khi gửi email.", true, "SmtpUseSsl", "Boolean", null, null, "true" },
                    { 23, "Email", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9493), null, "My Application Support", "Tên hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromName", "Text", null, null, "My Application Support" },
                    { 24, "Email", new DateTime(2025, 4, 6, 17, 1, 22, 133, DateTimeKind.Utc).AddTicks(9494), null, "noreply@example.com", "Địa chỉ email hiển thị trong ô 'From' của email gửi đi.", true, "EmailFromAddress", "Email", null, null, "noreply@example.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "settings",
                keyColumn: "Id",
                keyValue: 24);
        }
    }
}
