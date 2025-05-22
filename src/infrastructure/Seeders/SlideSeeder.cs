using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class SlideSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public SlideSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 10;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Slides...");

        if (await _dbContext.Slides.AnyAsync())
        {
            return;
        }

        var slides = new List<Slide>
        {
            new Slide
            {
                Title = "CHỐNG THẤM ĐỈNH CAO",
                Subtitle = "Bảo vệ ngôi nhà của bạn khỏi mọi tác nhân gây hại",
                Description = "Sản phẩm chống thấm công nghệ Nhật Bản, cam kết hiệu quả lâu dài. Ưu đãi 20% tháng này!",
                ImageUrl = SeederHelpers.GetRandomImageUrl(1920, 800),
                CtaText = "Tìm hiểu thêm",
                CtaLink = "/san-pham/chong-tham",
                Target = "_self",
                OrderIndex = 1,
                IsActive = true,
                StartAt = DateTime.Now.AddDays(-30),
                EndAt = DateTime.Now.AddDays(30),
                CreatedAt = DateTime.Now
            },
            new Slide
            {
                Title = "SƠN NỘI THẤT HOÀN HẢO",
                Subtitle = "Không gian sống sang trọng, tinh tế",
                Description = "Hàng ngàn màu sắc thời thượng, an toàn cho sức khỏe, dễ dàng vệ sinh. Miễn phí tư vấn phối màu.",
                ImageUrl = SeederHelpers.GetRandomImageUrl(1920, 800),
                CtaText = "Xem bộ sưu tập",
                CtaLink = "/san-pham/son-noi-that",
                Target = "_self",
                OrderIndex = 2,
                IsActive = true,
                StartAt = DateTime.Now.AddDays(-15),
                EndAt = DateTime.Now.AddDays(45),
                CreatedAt = DateTime.Now
            },
            new Slide
            {
                Title = "DỰ ÁN TIÊU BIỂU",
                Subtitle = "Thành công từ sự tin tưởng",
                Description = "Chúng tôi tự hào là đối tác của nhiều dự án lớn nhỏ trên toàn quốc. Xem các công trình đã hoàn thành.",
                ImageUrl = SeederHelpers.GetRandomImageUrl(1920, 800),
                CtaText = "Khám phá",
                CtaLink = "/du-an", // Giả định có trang dự án
                Target = "_self",
                OrderIndex = 3,
                IsActive = true,
                StartAt = DateTime.Now.AddDays(-45),
                EndAt = DateTime.Now.AddDays(15),
                CreatedAt = DateTime.Now
            },
            new Slide
            {
                Title = "KHUYẾN MÃI LỚN",
                Subtitle = "Mua 2 tặng 1 cho sản phẩm chọn lọc",
                Description = "Cơ hội vàng để sở hữu các sản phẩm sơn và chống thấm chất lượng cao với giá ưu đãi. Áp dụng đến hết tháng!",
                ImageUrl = SeederHelpers.GetRandomImageUrl(1920, 800),
                CtaText = "Mua ngay",
                CtaLink = "/khuyen-mai",
                Target = "_self",
                OrderIndex = 4,
                IsActive = true,
                StartAt = DateTime.Now.AddDays(-7),
                EndAt = DateTime.Now.AddDays(23),
                CreatedAt = DateTime.Now
            }
        };

        await _dbContext.Slides.AddRangeAsync(slides);
        await _dbContext.SaveChangesAsync();
    }
}
