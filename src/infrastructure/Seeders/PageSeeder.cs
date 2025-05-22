using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Enums;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class PageSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public PageSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 11;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Pages...");

        if (await _dbContext.Pages.AnyAsync())
        {
            return; // Already seeded
        }

        var pages = new List<Page>
        {
            new Page
            {
                Title = "Giới Thiệu Về Chúng Tôi",
                Slug = SlugHelper.Generate("Giới Thiệu Về Chúng Tôi"),
                Content = "<p><strong>Công ty Sơn & Chống Thấm ABC</strong> tự hào là nhà cung cấp hàng đầu các giải pháp về sơn và chống thấm tại Việt Nam. Với nhiều năm kinh nghiệm trong ngành, chúng tôi cam kết mang đến những sản phẩm chất lượng cao, dịch vụ chuyên nghiệp và giải pháp tối ưu nhất cho mọi công trình.</p><p>Chúng tôi hiểu rằng mỗi công trình là một tác phẩm, và việc bảo vệ cũng như làm đẹp nó là sứ mệnh của chúng tôi. Đội ngũ kỹ sư và nhân viên lành nghề của chúng tôi luôn sẵn sàng tư vấn, hỗ trợ quý khách hàng từ khâu chọn lựa sản phẩm đến thi công hoàn thiện.</p>",
                Status = PublishStatus.Published,
                PublishedAt = SeederHelpers.GetRandomDateInPast(365, 730),
                MetaTitle = "Về Chúng Tôi - Công Ty Sơn & Chống Thấm ABC",
                MetaDescription = "Tìm hiểu về công ty Sơn & Chống Thấm ABC, nhà cung cấp giải pháp sơn và chống thấm hàng đầu Việt Nam.",
                MetaKeywords = "giới thiệu công ty, sơn chống thấm, abc paint",
                CreatedAt = DateTime.Now.AddDays(-730)
            },
            new Page
            {
                Title = "Chính Sách Bảo Mật",
                Slug = SlugHelper.Generate("Chính Sách Bảo Mật"),
                Content = "<p>Chúng tôi cam kết bảo mật tuyệt đối mọi thông tin cá nhân của khách hàng. Mọi dữ liệu thu thập được sử dụng duy nhất với mục đích cải thiện dịch vụ và trải nghiệm của bạn trên website.</p><p>Chúng tôi không chia sẻ thông tin cá nhân của bạn với bất kỳ bên thứ ba nào khi chưa có sự đồng ý của bạn, trừ trường hợp pháp luật yêu cầu.</p>",
                Status = PublishStatus.Published,
                PublishedAt = SeederHelpers.GetRandomDateInPast(180, 360),
                MetaTitle = "Chính Sách Bảo Mật - Sơn & Chống Thấm ABC",
                MetaDescription = "Đọc chính sách bảo mật thông tin cá nhân của khách hàng tại Sơn & Chống Thấm ABC.",
                MetaKeywords = "chính sách bảo mật, bảo mật thông tin, quyền riêng tư",
                CreatedAt = DateTime.Now.AddDays(-360)
            },
            new Page
            {
                Title = "Điều Khoản Dịch Vụ",
                Slug = SlugHelper.Generate("Điều Khoản Dịch Vụ"),
                Content = "<p>Khi sử dụng dịch vụ của chúng tôi, bạn đồng ý tuân thủ các điều khoản và điều kiện sau đây...</p><p>Chúng tôi có quyền thay đổi các điều khoản này bất cứ lúc nào mà không cần thông báo trước. Bạn có trách nhiệm tự mình cập nhật thông tin.</p>",
                Status = PublishStatus.Published,
                PublishedAt = SeederHelpers.GetRandomDateInPast(180, 360),
                MetaTitle = "Điều Khoản Dịch Vụ - Sơn & Chống Thấm ABC",
                MetaDescription = "Tìm hiểu các điều khoản và điều kiện khi sử dụng dịch vụ của Sơn & Chống Thấm ABC.",
                MetaKeywords = "điều khoản dịch vụ, quy định sử dụng, cam kết",
                CreatedAt = DateTime.Now.AddDays(-350)
            }
        };

        await _dbContext.Pages.AddRangeAsync(pages);
        await _dbContext.SaveChangesAsync();
    }
}
