using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Enums;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class SettingSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public SettingSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 16;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Settings...");

        if (await _dbContext.Settings.AnyAsync())
        {
            return; // Already seeded
        }

        var settings = new List<Setting>
        {
            // General Settings
            new Setting { Key = "SiteName", Category = "General", Type = FieldType.Text, Description = "Tên website chính", DefaultValue = "Sơn & Chống Thấm ABC", Value = "Sơn & Chống Thấm ABC", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "SiteSlogan", Category = "General", Type = FieldType.Text, Description = "Slogan của website", DefaultValue = "Giải pháp toàn diện cho mọi công trình", Value = "Giải pháp toàn diện cho mọi công trình", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "DefaultCurrency", Category = "General", Type = FieldType.Text, Description = "Đơn vị tiền tệ mặc định", DefaultValue = "VND", Value = "VND", IsActive = true, CreatedAt = DateTime.Now },

            // Contact Information
            new Setting { Key = "CompanyAddress", Category = "Contact", Type = FieldType.TextArea, Description = "Địa chỉ công ty", DefaultValue = "123 Đường ABC, Phường XYZ, Quận 1, TP. Hồ Chí Minh", Value = "123 Đường ABC, Phường XYZ, Quận 1, TP. Hồ Chí Minh", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "CompanyPhone", Category = "Contact", Type = FieldType.Text, Description = "Số điện thoại liên hệ chính", DefaultValue = "028.123.4567", Value = "028.123.4567", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "CompanyEmail", Category = "Contact", Type = FieldType.Text, Description = "Email liên hệ chính", DefaultValue = "info@sonchongtham.com", Value = "info@sonchongtham.com", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "WorkingHours", Category = "Contact", Type = FieldType.Text, Description = "Giờ làm việc", DefaultValue = "Thứ 2 - Thứ 7: 8h00 - 17h00", Value = "Thứ 2 - Thứ 7: 8h00 - 17h00", IsActive = true, CreatedAt = DateTime.Now },

            // Social Media
            new Setting { Key = "FacebookUrl", Category = "Social Media", Type = FieldType.Text, Description = "Đường dẫn Facebook", DefaultValue = "https://www.facebook.com/sonchongthamabc", Value = "https://www.facebook.com/sonchongthamabc", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "YoutubeUrl", Category = "Social Media", Type = FieldType.Text, Description = "Đường dẫn Youtube", DefaultValue = "https://www.youtube.com/c/sonchongthamabc", Value = "https://www.youtube.com/c/sonchongthamabc", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "ZaloPhone", Category = "Social Media", Type = FieldType.Text, Description = "Số điện thoại Zalo", DefaultValue = "0987654321", Value = "0987654321", IsActive = true, CreatedAt = DateTime.Now },

            // SEO Settings
            new Setting { Key = "HomePageMetaTitle", Category = "SEO", Type = FieldType.Text, Description = "Meta Title trang chủ", DefaultValue = "Sơn & Chống Thấm ABC - Chuyên gia hàng đầu về sơn và chống thấm", Value = "Sơn & Chống Thấm ABC - Chuyên gia hàng đầu về sơn và chống thấm", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "HomePageMetaDescription", Category = "SEO", Type = FieldType.TextArea, Description = "Meta Description trang chủ", DefaultValue = "Cung cấp đa dạng các sản phẩm sơn nội thất, ngoại thất, sơn chống thấm và vật liệu chống thấm chính hãng. Dịch vụ thi công chuyên nghiệp.", Value = "Cung cấp đa dạng các sản phẩm sơn nội thất, ngoại thất, sơn chống thấm và vật liệu chống thấm chính hãng. Dịch vụ thi công chuyên nghiệp.", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "GoogleAnalyticsId", Category = "Analytics", Type = FieldType.Text, Description = "Mã Google Analytics", DefaultValue = "UA-XXXXXXXXX-Y", Value = "G-XXXXXXXXXX", IsActive = true, CreatedAt = DateTime.Now },

            // Other settings
            new Setting { Key = "AllowNewsletterSubscription", Category = "Email", Type = FieldType.Boolean, Description = "Cho phép đăng ký nhận tin tức", DefaultValue = "true", Value = "true", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "MinOrderValueFreeShipping", Category = "Shipping", Type = FieldType.Number, Description = "Giá trị đơn hàng tối thiểu để được miễn phí vận chuyển", DefaultValue = "5000000", Value = "5000000", IsActive = true, CreatedAt = DateTime.Now },
            new Setting { Key = "FeaturedProductsCount", Category = "ProductDisplay", Type = FieldType.Number, Description = "Số lượng sản phẩm nổi bật hiển thị trên trang chủ", DefaultValue = "8", Value = "8", IsActive = true, CreatedAt = DateTime.Now }
        };

        await _dbContext.Settings.AddRangeAsync(settings);
        await _dbContext.SaveChangesAsync();
    }
}
