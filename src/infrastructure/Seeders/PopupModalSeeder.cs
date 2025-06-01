using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Enums; // Thêm using
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
            return;
        }

        var popupModals = new List<PopupModal>
        {
            new PopupModal
            {
                Title = "Đăng Ký Nhận Tư Vấn Miễn Phí!",
                Content = "<p>Để lại thông tin để nhận tư vấn...</p>",
                ImageUrl = SeederHelpers.GetRandomImageUrl(500, 300),
                LinkUrl = "/lien-he",
                IsActive = true,
                StartDate = DateTime.Now.AddDays(-7),
                EndDate = DateTime.Now.AddDays(30),
                TargetPages = "HomePage, /san-pham/*",
                DisplayFrequency = DisplayFrequency.Once,
                OrderIndex = 1,
                DelaySeconds = 5,
                CreatedAt = DateTime.Now
            },
            new PopupModal
            {
                Title = "Ưu Đãi Đặc Biệt: Miễn Phí Vận Chuyển!",
                Content = "<p>Nhận ngay ưu đãi miễn phí vận chuyển...</p>",
                ImageUrl = SeederHelpers.GetRandomImageUrl(500, 300),
                LinkUrl = "/san-pham",
                IsActive = true,
                StartDate = DateTime.Now.AddDays(-10),
                EndDate = DateTime.Now.AddDays(20),
                TargetPages = "AllPages",
                DisplayFrequency = DisplayFrequency.Always,
                OrderIndex = 2,
                DelaySeconds = 2,
                CreatedAt = DateTime.Now
            },
        };

        await _dbContext.PopupModals.AddRangeAsync(popupModals);
        await _dbContext.SaveChangesAsync();
    }
}