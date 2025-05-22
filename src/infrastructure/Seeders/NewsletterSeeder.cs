using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class NewsletterSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public NewsletterSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 14;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Newsletters...");

        if (await _dbContext.Newsletters.AnyAsync())
        {
            return; // Already seeded
        }

        var newsletters = new List<Newsletter>
        {
            new Newsletter
            {
                Email = SeederHelpers.GenerateRandomEmail(),
                Name = "Trần Bích Thủy",
                IsActive = true,
                IpAddress = "192.168.1.100",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/91.0.4472.124",
                ConfirmedAt = SeederHelpers.GetRandomDateInPast(60, 180),
                CreatedAt = SeederHelpers.GetRandomDateInPast(180, 365)
            },
            new Newsletter
            {
                Email = SeederHelpers.GenerateRandomEmail(),
                Name = "Lê Văn Tùng",
                IsActive = true,
                IpAddress = "192.168.1.101",
                UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) Safari/537.36",
                ConfirmedAt = SeederHelpers.GetRandomDateInPast(30, 90),
                CreatedAt = SeederHelpers.GetRandomDateInPast(90, 270)
            },
            new Newsletter
            {
                Email = SeederHelpers.GenerateRandomEmail(),
                Name = null, // No name provided
                IsActive = true,
                IpAddress = "192.168.1.102",
                UserAgent = "Mozilla/5.0 (Linux; Android 10) Firefox/89.0",
                ConfirmedAt = SeederHelpers.GetRandomDateInPast(1, 30),
                CreatedAt = SeederHelpers.GetRandomDateInPast(30, 120)
            },
            new Newsletter
            {
                Email = SeederHelpers.GenerateRandomEmail(),
                Name = "Đỗ Duy Mạnh",
                IsActive = false, // Unsubscribed
                IpAddress = "192.168.1.103",
                UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 14_6 like Mac OS X)",
                ConfirmedAt = SeederHelpers.GetRandomDateInPast(120, 240),
                UnsubscribedAt = SeederHelpers.GetRandomDateInPast(1, 10),
                CreatedAt = SeederHelpers.GetRandomDateInPast(240, 400)
            },
            new Newsletter
            {
                Email = SeederHelpers.GenerateRandomEmail(),
                Name = "Nguyễn Thị Hoa",
                IsActive = true,
                IpAddress = "192.168.1.104",
                UserAgent = "Mozilla/5.0 (iPad; CPU OS 13_5 like Mac OS X)",
                ConfirmedAt = SeederHelpers.GetRandomDateInPast(5, 20),
                CreatedAt = SeederHelpers.GetRandomDateInPast(20, 60)
            }
        };

        await _dbContext.Newsletters.AddRangeAsync(newsletters);
        await _dbContext.SaveChangesAsync();
    }
}

