using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class PopupModalSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public PopupModalSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 12;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Popup Modals...");

        if (await _dbContext.PopupModals.AnyAsync())
        {
            return; // Already seeded
        }

        var popupModals = new List<PopupModal>
        {
            new PopupModal
            {
                Title = "Đăng Ký Nhận Tư Vấn Miễn Phí!",
                Content = "<p>Để lại thông tin để nhận được tư vấn chuyên sâu về các giải pháp sơn và chống thấm phù hợp nhất cho công trình của bạn.</p><p><strong>Đăng ký ngay hôm nay để không bỏ lỡ!</strong></p>",
                ImageUrl = SeederHelpers.GetRandomImageUrl(500, 300),
                LinkUrl = "/lien-he",
                IsActive = true,
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now.AddDays(30),
                CreatedAt = DateTime.Now
            },
            new PopupModal
            {
                Title = "Ưu Đãi Đặc Biệt: Miễn Phí Vận Chuyển!",
                Content = "<p>Nhận ngay ưu đãi miễn phí vận chuyển cho tất cả các đơn hàng trên 5 triệu VNĐ. Áp dụng cho toàn quốc.</p><p>Thời gian có hạn, nhanh tay đặt hàng!</p>",
                ImageUrl = SeederHelpers.GetRandomImageUrl(500, 300),
                LinkUrl = "/san-pham",
                IsActive = true,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(20),
                CreatedAt = DateTime.Now
            },
            new PopupModal
            {
                Title = "Chào Mừng Thành Viên Mới!",
                Content = "<p>Giảm ngay 10% cho đơn hàng đầu tiên khi bạn đăng ký tài khoản mới trên website của chúng tôi.</p>",
                ImageUrl = SeederHelpers.GetRandomImageUrl(500, 300),
                LinkUrl = "/dang-ky",
                IsActive = true,
                StartDate = null, // Always active
                EndDate = null, // Always active
                CreatedAt = DateTime.Now
            },
            new PopupModal
            {
                Title = "Popup Đã Hết Hạn",
                Content = "Đây là popup đã hết hạn và sẽ không hiển thị.",
                ImageUrl = SeederHelpers.GetRandomImageUrl(500, 300),
                LinkUrl = "#",
                IsActive = false, // Inactive example
                StartDate = DateTime.Now.AddDays(-60),
                EndDate = DateTime.Now.AddDays(-30),
                CreatedAt = DateTime.Now.AddDays(-60)
            }
        };

        await _dbContext.PopupModals.AddRangeAsync(popupModals);
        await _dbContext.SaveChangesAsync();
    }
}
