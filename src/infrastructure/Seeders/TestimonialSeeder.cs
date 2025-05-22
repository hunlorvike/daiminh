using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class TestimonialSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public TestimonialSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 13;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Testimonials...");

        if (await _dbContext.Testimonials.AnyAsync())
        {
            return; // Already seeded
        }

        var testimonials = new List<Testimonial>
        {
            new Testimonial
            {
                ClientName = "Nguyễn Văn Hùng",
                ClientTitle = "Chủ đầu tư biệt thự",
                ClientCompany = "Dự án An Gia",
                ClientAvatar = SeederHelpers.GetRandomAvatarUrl(),
                Content = "Tôi rất hài lòng với chất lượng sơn Dulux và dịch vụ thi công chuyên nghiệp từ Sơn & Chống Thấm ABC. Biệt thự của tôi đã hoàn thiện rất đẹp và bền màu.",
                Rating = 5,
                IsActive = true,
                OrderIndex = 1,
                CreatedAt = DateTime.Now
            },
            new Testimonial
            {
                ClientName = "Phạm Thị Thu",
                ClientTitle = "Khách hàng cá nhân",
                ClientCompany = null,
                ClientAvatar = SeederHelpers.GetRandomAvatarUrl(),
                Content = "Sản phẩm chống thấm Kova CT-11A Gold thật sự hiệu quả. Tường nhà tôi trước đây bị ẩm mốc nặng nay đã khô ráo hoàn toàn. Cảm ơn Sơn & Chống Thấm ABC đã tư vấn nhiệt tình!",
                Rating = 5,
                IsActive = true,
                OrderIndex = 2,
                CreatedAt = DateTime.Now
            },
            new Testimonial
            {
                ClientName = "Lê Hoàng Phúc",
                ClientTitle = "Giám đốc kỹ thuật",
                ClientCompany = "Công ty Xây dựng Minh Long",
                ClientAvatar = SeederHelpers.GetRandomAvatarUrl(),
                Content = "Giải pháp sơn epoxy sàn của Sika và sự hỗ trợ kỹ thuật từ đội ngũ Sơn & Chống Thấm ABC đã giúp nhà xưởng của chúng tôi có bề mặt sàn tuyệt vời, bền bỉ với thời gian.",
                Rating = 4,
                IsActive = true,
                OrderIndex = 3,
                CreatedAt = DateTime.Now
            },
            new Testimonial
            {
                ClientName = "Đỗ Thị Mai",
                ClientTitle = "Chủ nhà",
                ClientCompany = null,
                ClientAvatar = SeederHelpers.GetRandomAvatarUrl(),
                Content = "Sơn Mykolor Grand NanoClean đúng như quảng cáo, rất dễ lau chùi và không mùi. Tôi rất thích màu sắc và không gian thoáng đãng sau khi sơn lại nhà.",
                Rating = 5,
                IsActive = true,
                OrderIndex = 4,
                CreatedAt = DateTime.Now
            },
            new Testimonial
            {
                ClientName = "Trần Đình An",
                ClientTitle = "Kỹ sư xây dựng",
                ClientCompany = "Tập đoàn Phát triển Bất động sản Đất Việt",
                ClientAvatar = SeederHelpers.GetRandomAvatarUrl(),
                Content = "Chúng tôi tin dùng sản phẩm của Sơn & Chống Thấm ABC cho các dự án của mình. Chất lượng sản phẩm luôn ổn định và dịch vụ giao hàng rất nhanh chóng.",
                Rating = 5,
                IsActive = true,
                OrderIndex = 5,
                CreatedAt = DateTime.Now
            }
        };

        await _dbContext.Testimonials.AddRangeAsync(testimonials);
        await _dbContext.SaveChangesAsync();
    }
}
