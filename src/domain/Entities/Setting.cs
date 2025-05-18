using domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using shared.Enums;

namespace domain.Entities;

public class Setting : BaseEntity<int>
{
    public string Key { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty; // e.g., "General", "Email", "Social Media", "Payment", "SEO", "Contact", "Company Info", "Theme", "Analytics", "Security", "Cache", "API", "Custom"
    public FieldType Type { get; set; }
    public string? Description { get; set; }
    public string? DefaultValue { get; set; }
    public string? Value { get; set; }
    public bool IsActive { get; set; } = true;
}

public class SettingConfiguration : BaseEntityConfiguration<Setting, int>
{
    public override void Configure(EntityTypeBuilder<Setting> builder)
    {
        base.Configure(builder);
        builder.Property(s => s.Key).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Category).IsRequired().HasMaxLength(50);
        builder.HasIndex(s => new { s.Key, s.Category }).IsUnique();
        builder.Property(e => e.Type)
            .IsRequired()
            .HasDefaultValue(FieldType.Text);
        builder.Property(s => s.Description).HasMaxLength(500);
        builder.Property(s => s.DefaultValue).HasColumnType("nvarchar(max)");
        builder.Property(s => s.Value).HasColumnType("nvarchar(max)");
        builder.Property(s => s.IsActive).HasDefaultValue(true);
        builder.HasData(
            // General Settings (ID 1-10)
            new Setting
            {
                Id = 1,
                Key = "SiteName",
                Category = "General",
                Type = FieldType.Text,
                Description = "Tên website hiển thị trên trang và tiêu đề trình duyệt.",
                DefaultValue = "Sơn Đại Minh Việt Nam", // Cập nhật
                Value = "Sơn Đại Minh Việt Nam",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Setting
            {
                Id = 2,
                Key = "SiteUrl",
                Category = "General",
                Type = FieldType.Url,
                Description = "Địa chỉ URL chính của website (ví dụ: https://www.example.com).",
                DefaultValue = "https://localhost:7001", // Giữ localhost cho dev, cập nhật khi deploy
                Value = "https://localhost:7001",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Setting
            {
                Id = 3,
                Key = "AdminEmail",
                Category = "General",
                Type = FieldType.Email,
                Description = "Địa chỉ email quản trị viên để nhận thông báo hệ thống (đơn hàng, liên hệ...).",
                DefaultValue = "sondaiminh@gmail.com",
                Value = "sondaiminh@gmail.com",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Setting
            {
                Id = 4,
                Key = "LogoUrl",
                Category = "General",
                Type = FieldType.Image,
                Description = "Đường dẫn đến file logo website.",
                DefaultValue = "/images/logo.png", // Placeholder
                Value = "/images/logo.png",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
             new Setting
             {
                 Id = 5,
                 Key = "FaviconUrl", // Di chuyển Favicon về General
                 Category = "General",
                 Type = FieldType.Image,
                 Description = "Đường dẫn đến file favicon.ico hoặc ảnh favicon.",
                 DefaultValue = "/images/favicon.ico", // Placeholder
                 Value = "/images/favicon.ico",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
            // Additional General Settings
            new Setting
            {
                Id = 6,
                Key = "ItemsPerPage",
                Category = "General",
                Type = FieldType.Number,
                Description = "Số lượng sản phẩm/bài viết hiển thị trên mỗi trang danh sách mặc định.",
                DefaultValue = "12",
                Value = "12",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },


             // Contact Settings (ID 11-20)
             new Setting
             {
                 Id = 11,
                 Key = "CompanyName",
                 Category = "Contact",
                 Type = FieldType.Text,
                 Description = "Tên công ty hoặc tổ chức sở hữu website.",
                 DefaultValue = "Sơn Đại Minh", // Cập nhật tên công ty
                 Value = "Sơn Đại Minh",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 12,
                 Key = "ContactAddress",
                 Category = "Contact",
                 Type = FieldType.TextArea,
                 Description = "Địa chỉ liên hệ đầy đủ của công ty.",
                 DefaultValue = "Tiên Phương, Chương Mỹ, Hà Nội", // Cập nhật địa chỉ
                 Value = "Tiên Phương, Chương Mỹ, Hà Nội",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 13,
                 Key = "ContactPhone",
                 Category = "Contact",
                 Type = FieldType.Phone,
                 Description = "Số điện thoại liên hệ chung.",
                 DefaultValue = "0979758340", // Cập nhật SĐT
                 Value = "0979758340",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 14,
                 Key = "ContactEmail",
                 Category = "Contact",
                 Type = FieldType.Email,
                 Description = "Địa chỉ email hiển thị công khai để liên hệ.",
                 DefaultValue = "sondaiminh@gmail.com", // Sử dụng email admin làm email liên hệ công khai
                 Value = "sondaiminh@gmail.com",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 15,
                 Key = "ContactMapEmbed",
                 Category = "Contact",
                 Type = FieldType.Html,
                 Description = "Mã nhúng HTML của bản đồ (ví dụ: Google Maps iframe) hiển thị trên trang Liên hệ.",
                 // Cập nhật iframe map
                 DefaultValue = @"<iframe src=""https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5152.719628304902!2d105.68369562421606!3d20.94205043073292!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3134539b465b1be5%3A0x279c032890c390c5!2zxJDhuqFpIE1pbmggVmnhu4d0IE5hbQ!5e1!3m2!1svi!2s!4v1745895737603!5m2!1svi!2s"" width=""600"" height=""450"" style=""border:0;"" allowfullscreen="""" loading=""lazy"" referrerpolicy=""no-referrer-when-downgrade""></iframe>",
                 Value = @"<iframe src=""https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d5152.719628304902!2d105.68369562421606!3d20.94205043073292!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3134539b465b1be5%3A0x279c032890c390c5!2zxJDhuqFpIE1pbmggVmnhu4d0IE5hbQ!5e1!3m2!1svi!2s!4v1745895737603!5m2!1svi!2s"" width=""600"" height=""450"" style=""border:0;"" allowfullscreen="""" loading=""lazy"" referrerpolicy=""no-referrer-when-downgrade""></iframe>",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
            // Additional Contact Settings
            new Setting
            {
                Id = 16,
                Key = "HotlinePhone",
                Category = "Contact",
                Type = FieldType.Phone,
                Description = "Số điện thoại Hotline hỗ trợ nhanh.",
                DefaultValue = "0979758340", // Cập nhật SĐT Hotline
                Value = "0979758340",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
             new Setting
             {
                 Id = 17,
                 Key = "ContactWorkingHours",
                 Category = "Contact",
                 Type = FieldType.Text,
                 Description = "Giờ làm việc của công ty.",
                 DefaultValue = "Thứ 2 - Thứ 7: 8h00 - 17h00",
                 Value = "Thứ 2 - Thứ 7: 8h00 - 17h00",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 18,
                 Key = "TaxId",
                 Category = "Contact",
                 Type = FieldType.Text,
                 Description = "Mã số thuế của công ty.",
                 DefaultValue = null, // Cần điền thông tin thật
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },


             // SEO Settings (ID 21-30) - Meta title/description đã có trong SeoEntity base, nhưng setting này là mặc định toàn trang
             new Setting
             {
                 Id = 21,
                 Key = "DefaultMetaTitle",
                 Category = "SEO",
                 Type = FieldType.Text,
                 Description = "Tiêu đề meta mặc định cho các trang không có tiêu đề riêng.",
                 DefaultValue = "Sơn Chống Thấm, Vật Liệu Sơn Chính Hãng | Đại Minh Việt Nam", // Cập nhật
                 Value = "Sơn Chống Thấm, Vật Liệu Sơn Chính Hãng | Đại Minh Việt Nam",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 22,
                 Key = "DefaultMetaDescription",
                 Category = "SEO",
                 Type = FieldType.TextArea,
                 Description = "Mô tả meta mặc định (dưới 160 ký tự) cho các trang không có mô tả riêng.",
                 DefaultValue = "Đại Minh Việt Nam chuyên cung cấp sơn, vật liệu chống thấm, phụ gia bê tông chính hãng từ các thương hiệu hàng đầu. Tư vấn giải pháp thi công hiệu quả. Liên hệ 0979758340.", // Cập nhật
                 Value = "Đại Minh Việt Nam chuyên cung cấp sơn, vật liệu chống thấm, phụ gia bê tông chính hãng từ các thương hiệu hàng đầu. Tư vấn giải pháp thi công hiệu quả. Liên hệ 0979758340.",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 23,
                 Key = "GoogleAnalyticsId",
                 Category = "SEO",
                 Type = FieldType.Text,
                 Description = "Mã ID Google Analytics (ví dụ: UA-XXXXXXX-Y hoặc G-XXXXXXXXXX).",
                 DefaultValue = null, // Cần điền thông tin thật
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 24,
                 Key = "VerificationMetaTags",
                 Category = "SEO",
                 Type = FieldType.TextArea,
                 Description = "Các meta tag xác minh website (Google, Bing, ...).",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },

             // Social Media Settings (ID 31-40)
             new Setting
             {
                 Id = 31,
                 Key = "SocialFacebookUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang Facebook của công ty.",
                 DefaultValue = "https://www.facebook.com/LienDaiMinh", // Cập nhật
                 Value = "https://www.facebook.com/LienDaiMinh",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 32,
                 Key = "SocialTwitterUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang Twitter (X) của công ty.",
                 DefaultValue = null, // Giữ null theo yêu cầu
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 33,
                 Key = "SocialInstagramUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang Instagram của công ty.",
                 DefaultValue = null, // Giữ null theo yêu cầu
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 34,
                 Key = "SocialLinkedInUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang LinkedIn của công ty.",
                 DefaultValue = null, // Giữ null theo yêu cầu
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 35,
                 Key = "SocialYoutubeUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL kênh Youtube của công ty.",
                 DefaultValue = null, // Giữ null theo yêu cầu
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 36,
                 Key = "SocialTiktokUrl", // ID mới
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL kênh Tiktok của công ty.",
                 DefaultValue = "https://www.tiktok.com/@hung.daiminh", // Cập nhật
                 Value = "https://www.tiktok.com/@hung.daiminh",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
            new Setting
            {
                Id = 37,
                Key = "SocialZaloPhone",
                Category = "Social Media",
                Type = FieldType.Phone,
                Description = "Số điện thoại Zalo để liên hệ nhanh (có thể khác Hotline).",
                DefaultValue = "0979758340", // Cập nhật
                Value = "0979758340",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },


             // Email Settings (ID 41-50) - Cần cấu hình SMTP thật qua UI/secrets
             new Setting
             {
                 Id = 41,
                 Key = "SmtpHost",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "Địa chỉ máy chủ SMTP để gửi email.",
                 DefaultValue = "smtp.example.com", // Giữ placeholder
                 Value = "smtp.example.com",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 42,
                 Key = "SmtpPort",
                 Category = "Email",
                 Type = FieldType.Number,
                 Description = "Cổng SMTP (ví dụ: 587, 465, 25).",
                 DefaultValue = "587",
                 Value = "587",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 43,
                 Key = "SmtpUsername",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "Tên đăng nhập SMTP.",
                 DefaultValue = "user@example.com", // Giữ placeholder
                 Value = "user@example.com",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 44,
                 Key = "SmtpPassword",
                 Category = "Email",
                 Type = FieldType.Text, // Sử dụng FieldType.Password nếu có
                 Description = "**QUAN TRỌNG**: Mật khẩu SMTP. Nên cấu hình qua UI, không seed giá trị thật.",
                 DefaultValue = null, // Giữ null, không seed mật khẩu thật
                 Value = null,
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 45,
                 Key = "SmtpUseSsl",
                 Category = "Email",
                 Type = FieldType.Boolean,
                 Description = "Sử dụng mã hóa SSL/TLS khi gửi email.",
                 DefaultValue = "true",
                 Value = "true",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 46,
                 Key = "EmailFromName",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "Tên hiển thị trong ô 'From' của email gửi đi.",
                 DefaultValue = "Đại Minh Việt Nam", // Cập nhật
                 Value = "Đại Minh Việt Nam",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 47,
                 Key = "EmailFromAddress",
                 Category = "Email",
                 Type = FieldType.Email,
                 Description = "Địa chỉ email hiển thị trong ô 'From' của email gửi đi.",
                 DefaultValue = "noreply@daiminhvietnam.com", // Sử dụng domain ví dụ, thay đổi khi deploy
                 Value = "noreply@daiminhvietnam.com",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             // Additional Email Settings (Template paths/keys)
             new Setting
             {
                 Id = 48,
                 Key = "EmailTemplateContactFormReply",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "Mã/tên template email phản hồi tự động khi nhận form liên hệ.",
                 DefaultValue = "ContactReply", // Example template name
                 Value = "ContactReply",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
            new Setting
            {
                Id = 49,
                Key = "EmailTemplateNewsletterSubscribe",
                Category = "Email",
                Type = FieldType.Text,
                Description = "Mã/tên template email xác nhận đăng ký nhận tin.",
                DefaultValue = "NewsletterSubscribe", // Example template name
                Value = "NewsletterSubscribe",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },


             // Appearance Settings (ID 51-60)
             new Setting
             {
                 Id = 51,
                 Key = "HomepageBanner1Url",
                 Category = "Appearance",
                 Type = FieldType.Image,
                 Description = "URL ảnh banner chính trang chủ.",
                 DefaultValue = "/images/banners/banner1.jpg", // Placeholder
                 Value = "/images/banners/banner1.jpg",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
            new Setting
            {
                Id = 52,
                Key = "HomepageBanner1Link",
                Category = "Appearance",
                Type = FieldType.Url,
                Description = "URL liên kết khi click banner chính trang chủ.",
                DefaultValue = "/", // Link về trang chủ
                Value = "/",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
             new Setting
             {
                 Id = 53,
                 Key = "CopyrightText",
                 Category = "Appearance",
                 Type = FieldType.Text,
                 Description = "Nội dung text copyright hiển thị ở chân trang.",
                 DefaultValue = $"© {DateTime.Now.Year} Sơn Đại Minh Việt Nam. All rights reserved.",
                 Value = $"© {DateTime.Now.Year} Sơn Đại Minh Việt Nam. All rights reserved.",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },


            // Integration Settings (ID 61-70)
            new Setting
            {
                Id = 61,
                Key = "LiveChatScript",
                Category = "Integration",
                Type = FieldType.Html,
                Description = "Mã script nhúng Live Chat (Zalo, Tawk.to, subiz...).",
                DefaultValue = null, // Cần điền mã nhúng thực tế
                Value = null,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },


             // E-commerce Settings (ID 71-80)
             new Setting
             {
                 Id = 71,
                 Key = "CurrencyCode",
                 Category = "E-commerce",
                 Type = FieldType.Text,
                 Description = "Mã tiền tệ chính được sử dụng (ví dụ: VND, USD).",
                 DefaultValue = "VND",
                 Value = "VND",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
             new Setting
             {
                 Id = 72,
                 Key = "CurrencySymbol",
                 Category = "E-commerce",
                 Type = FieldType.Text,
                 Description = "Ký hiệu tiền tệ hiển thị (ví dụ: đ, $).",
                 DefaultValue = "đ",
                 Value = "đ",
                 IsActive = true,
                 CreatedAt = DateTime.UtcNow
             },
            new Setting
            {
                Id = 73,
                Key = "DefaultProductImageUrl",
                Category = "E-commerce",
                Type = FieldType.Image,
                Description = "URL ảnh mặc định hiển thị khi sản phẩm không có ảnh.",
                DefaultValue = "/images/product-placeholder.png", // Placeholder
                Value = "/images/product-placeholder.png",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }

            // Add more settings here as needed, continuing the ID sequence
            // Example: Policy page links
            // new Setting { Id = 81, Key = "LinkPrivacyPolicy", Category = "General", Type = FieldType.Url, Description = "Link đến trang chính sách bảo mật.", DefaultValue = "/chinh-sach-bao-mat", Value = "/chinh-sach-bao-mat", IsActive = true, CreatedAt = DateTime.UtcNow },
            // new Setting { Id = 82, Key = "LinkTermsOfService", Category = "General", Type = FieldType.Url, Description = "Link đến trang điều khoản dịch vụ.", DefaultValue = "/dieu-khoan-dich-vu", Value = "/dieu-khoan-dich-vu", IsActive = true, CreatedAt = DateTime.UtcNow },
            // new Setting { Id = 83, Key = "LinkShippingPolicy", Category = "General", Type = FieldType.Url, Description = "Link đến trang chính sách vận chuyển.", DefaultValue = "/chinh-sach-van-chuyen", Value = "/chinh-sach-van-chuyen", IsActive = true, CreatedAt = DateTime.UtcNow },
        );
    }
}