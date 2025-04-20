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

    public string? Description { get; set; } // Description of the setting

    public string? DefaultValue { get; set; } // Default value for the setting

    public string? Value { get; set; } // Current value of the setting

    public bool IsActive { get; set; } = true; // Whether the setting is active
}

public class SettingConfiguration : IEntityTypeConfiguration<Setting>
{
    public void Configure(EntityTypeBuilder<Setting> builder)
    {
        builder.ToTable("settings");

        builder.Property(s => s.Key).HasColumnName("key").IsRequired().HasMaxLength(100);
        builder.Property(s => s.Category).HasColumnName("category").IsRequired().HasMaxLength(50);
        builder.Property(e => e.Type)
            .HasColumnName("type")
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue(FieldType.Text);
        builder.Property(s => s.Description).HasColumnName("description").HasMaxLength(500);
        builder.Property(s => s.DefaultValue).HasColumnName("default_value");
        builder.Property(s => s.Value).HasColumnName("value");
        builder.Property(s => s.IsActive).HasColumnName("is_active").HasDefaultValue(true);

        // Indexes
        builder.HasIndex(s => s.Key).HasDatabaseName("idx_settings_key").IsUnique();
        builder.HasIndex(s => s.Category).HasDatabaseName("idx_settings_category");
        builder.HasIndex(s => s.Type).HasDatabaseName("idx_settings_type");
        builder.HasIndex(s => s.IsActive).HasDatabaseName("idx_settings_is_active");

        // Seed data (optional)
        builder.HasData(
             new Setting
             {
                 Id = 1,
                 Key = "SiteName",
                 Category = "General",
                 Type = FieldType.Text,
                 Description = "Tên website hiển thị trên trang và tiêu đề trình duyệt.",
                 DefaultValue = "Đại Minh Việt Nam",
                 Value = "Đại Minh Việt Nam",
                 IsActive = true
             },
             new Setting
             {
                 Id = 2,
                 Key = "SiteUrl",
                 Category = "General",
                 Type = FieldType.Url,
                 Description = "Địa chỉ URL chính của website (ví dụ: https://www.example.com).",
                 DefaultValue = "https://localhost:7001",
                 Value = "https://localhost:7001",
                 IsActive = true
             },
             new Setting
             {
                 Id = 3,
                 Key = "AdminEmail",
                 Category = "General",
                 Type = FieldType.Email,
                 Description = "Địa chỉ email quản trị viên để nhận thông báo hệ thống.",
                 DefaultValue = "sondaiminh@gmail.com",
                 Value = "sondaiminh@gmail.com",
                 IsActive = true
             },
             new Setting
             {
                 Id = 5,
                 Key = "CompanyName",
                 Category = "Contact",
                 Type = FieldType.Text,
                 Description = "Tên công ty hoặc tổ chức sở hữu website.",
                 DefaultValue = "Đại Minh Việt Nam",
                 Value = "Đại Minh Việt Nam",
                 IsActive = true
             },
             new Setting
             {
                 Id = 6,
                 Key = "ContactAddress",
                 Category = "Contact",
                 Type = FieldType.TextArea,
                 Description = "Địa chỉ liên hệ đầy đủ.",
                 DefaultValue = "123 Main Street, Anytown, CA 91234",
                 Value = "123 Main Street, Anytown, CA 91234",
                 IsActive = true
             },
             new Setting
             {
                 Id = 7,
                 Key = "ContactPhone",
                 Category = "Contact",
                 Type = FieldType.Phone,
                 Description = "Số điện thoại liên hệ chính.",
                 DefaultValue = "(123) 456-7890",
                 Value = "(123) 456-7890",
                 IsActive = true
             },
             new Setting
             {
                 Id = 8,
                 Key = "ContactEmail",
                 Category = "Contact",
                 Type = FieldType.Email,
                 Description = "Địa chỉ email hiển thị công khai để liên hệ.",
                 DefaultValue = "contact@example.com",
                 Value = "contact@example.com",
                 IsActive = true
             },
             new Setting
             {
                 Id = 9,
                 Key = "ContactMapEmbed",
                 Category = "Contact",
                 Type = FieldType.Html,
                 Description = "Mã nhúng HTML của bản đồ (ví dụ: Google Maps iframe).",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true
             },
             new Setting
             {
                 Id = 10,
                 Key = "DefaultMetaTitle",
                 Category = "SEO",
                 Type = FieldType.Text,
                 Description = "Tiêu đề meta mặc định cho các trang không có tiêu đề riêng.",
                 DefaultValue = "Welcome to My Application",
                 Value = "Welcome to My Application",
                 IsActive = true
             },
             new Setting
             {
                 Id = 11,
                 Key = "DefaultMetaDescription",
                 Category = "SEO",
                 Type = FieldType.TextArea,
                 Description = "Mô tả meta mặc định (dưới 160 ký tự).",
                 DefaultValue = "This is the default description for My Application.",
                 Value = "This is the default description for My Application.",
                 IsActive = true
             },
             new Setting
             {
                 Id = 12,
                 Key = "FaviconUrl",
                 Category = "SEO",
                 Type = FieldType.Image,
                 Description = "Đường dẫn đến file favicon.ico hoặc ảnh favicon.",
                 DefaultValue = "/image/icon.jpg",
                 Value = "/image/icon.jpg",
                 IsActive = true
             },
             new Setting
             {
                 Id = 13,
                 Key = "SocialFacebookUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang Facebook.",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true
             },
             new Setting
             {
                 Id = 14,
                 Key = "SocialTwitterUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang Twitter (X).",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true
             },
             new Setting
             {
                 Id = 15,
                 Key = "SocialInstagramUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang Instagram.",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true
             },
             new Setting
             {
                 Id = 16,
                 Key = "SocialLinkedInUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL trang LinkedIn.",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true
             },
             new Setting
             {
                 Id = 17,
                 Key = "SocialYoutubeUrl",
                 Category = "Social Media",
                 Type = FieldType.Url,
                 Description = "URL kênh Youtube.",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true
             },
             new Setting
             {
                 Id = 18,
                 Key = "SmtpHost",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "Địa chỉ máy chủ SMTP.",
                 DefaultValue = "smtp.example.com",
                 Value = "smtp.example.com",
                 IsActive = true
             },
             new Setting
             {
                 Id = 19,
                 Key = "SmtpPort",
                 Category = "Email",
                 Type = FieldType.Number,
                 Description = "Cổng SMTP (ví dụ: 587, 465, 25).",
                 DefaultValue = "587",
                 Value = "587",
                 IsActive = true
             },
             new Setting
             {
                 Id = 20,
                 Key = "SmtpUsername",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "Tên đăng nhập SMTP.",
                 DefaultValue = "user@example.com",
                 Value = "user@example.com",
                 IsActive = true
             },
             new Setting
             {
                 Id = 21,
                 Key = "SmtpPassword",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "**QUAN TRỌNG**: Mật khẩu SMTP. Nên cấu hình qua UI, không seed giá trị thật.",
                 DefaultValue = null,
                 Value = null,
                 IsActive = true
             },
             new Setting
             {
                 Id = 22,
                 Key = "SmtpUseSsl",
                 Category = "Email",
                 Type = FieldType.Boolean,
                 Description = "Sử dụng mã hóa SSL/TLS khi gửi email.",
                 DefaultValue = "true",
                 Value = "true",
                 IsActive = true
             },
             new Setting
             {
                 Id = 23,
                 Key = "EmailFromName",
                 Category = "Email",
                 Type = FieldType.Text,
                 Description = "Tên hiển thị trong ô 'From' của email gửi đi.",
                 DefaultValue = "My Application Support",
                 Value = "My Application Support",
                 IsActive = true
             },
             new Setting
             {
                 Id = 24,
                 Key = "EmailFromAddress",
                 Category = "Email",
                 Type = FieldType.Email,
                 Description = "Địa chỉ email hiển thị trong ô 'From' của email gửi đi.",
                 DefaultValue = "noreply@example.com",
                 Value = "noreply@example.com",
                 IsActive = true
             }
         );
    }
}