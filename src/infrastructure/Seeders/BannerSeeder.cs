using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Enums;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class BannerSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public BannerSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 9;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Banners...");

        if (await _dbContext.Banners.AnyAsync())
        {
            return;
        }

        var banners = new List<Banner>
        {
            new Banner
            {
                Title = "Giảm Giá Lớn Sơn Nội Thất Dulux",
                Description = "Ưu đãi đặc biệt khi mua các sản phẩm sơn nội thất Dulux trong tháng này.",
                ImageUrl = SeederHelpers.GetRandomImageUrl(1920, 600),
                LinkUrl = "/san-pham/son-noi-that",
                Type = BannerType.Header,
                IsActive = true,
                OrderIndex = 1,
                CreatedAt = DateTime.Now
            },
            new Banner
            {
                Title = "Giải Pháp Chống Thấm Toàn Diện Cho Ngôi Nhà Bạn",
                Description = "Đừng để ẩm mốc làm phiền. Liên hệ ngay để được tư vấn miễn phí!",
                ImageUrl = SeederHelpers.GetRandomImageUrl(1920, 600),
                LinkUrl = "/dich-vu/chong-tham",
                Type = BannerType.Header,
                IsActive = true,
                OrderIndex = 2,
                CreatedAt = DateTime.Now
            },
            new Banner
            {
                Title = "Sơn Kova - Chất Lượng Bền Bỉ Với Thời Gian",
                Description = "Khám phá các sản phẩm sơn Kova chính hãng, giá tốt nhất.",
                ImageUrl = SeederHelpers.GetRandomImageUrl(1920, 600),
                LinkUrl = "/thuong-hieu/kova",
                Type = BannerType.Header,
                IsActive = true,
                OrderIndex = 3,
                CreatedAt = DateTime.Now
            },
            new Banner
            {
                Title = "Sản Phẩm Nổi Bật",
                Description = "Sơn Chống Thấm Kova CT-11A Gold",
                ImageUrl = SeederHelpers.GetRandomImageUrl(400, 300),
                LinkUrl = "/san-pham/son-chong-tham-kova-ct-11a-gold",
                Type = BannerType.Sidebar,
                IsActive = true,
                OrderIndex = 1,
                CreatedAt = DateTime.Now
            },
            new Banner
            {
                Title = "Bài Viết Mới Nhất",
                Description = "Hướng Dẫn Thi Công Sơn Chống Thấm Đúng Chuẩn",
                ImageUrl = SeederHelpers.GetRandomImageUrl(400, 300),
                LinkUrl = "/bai-viet/huong-dan-thi-cong-son-chong-tham-kova-ct-11a-dung-chuan",
                Type = BannerType.Sidebar,
                IsActive = true,
                OrderIndex = 2,
                CreatedAt = DateTime.Now
            }
        };

        await _dbContext.Banners.AddRangeAsync(banners);
        await _dbContext.SaveChangesAsync();
    }
}
