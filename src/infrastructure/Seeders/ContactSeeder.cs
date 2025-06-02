using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Enums;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class ContactSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public ContactSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public int Order => 15;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Contacts...");

        if (await _dbContext.Contacts.AnyAsync())
        {
            return; // Already seeded
        }

        var contacts = new List<Contact>
    {
        new Contact
        {
            FullName = "Nguyễn Hữu Trí",
            Email = SeederHelpers.GenerateRandomEmail(),
            Phone = SeederHelpers.GenerateRandomPhone(),
            Subject = "Yêu cầu báo giá sơn nội thất",
            Message = "Tôi muốn sơn lại căn hộ 70m2, xin vui lòng gửi báo giá các loại sơn nội thất cao cấp của Dulux và Mykolor. Cảm ơn!",
            Status = ContactStatus.New,
            IpAddress = "192.168.1.105",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Chrome/92.0.4515.159",
            CreatedAt = SeederHelpers.GetRandomDateInPast(1, 10)
        },
        new Contact
        {
            FullName = "Phạm Thị Huyền Trang",
            Email = SeederHelpers.GenerateRandomEmail(),
            Phone = SeederHelpers.GenerateRandomPhone(),
            Subject = "Tư vấn chống thấm sân thượng",
            Message = "Sân thượng nhà tôi đang bị thấm dột sau mỗi trận mưa. Tôi cần được tư vấn về giải pháp chống thấm hiệu quả nhất. Diện tích khoảng 50m2.",
            Status = ContactStatus.InProgress,
            AdminNotes = "Đã liên hệ, hẹn gặp khảo sát vào thứ 5.",
            IpAddress = "192.168.1.106",
            UserAgent = "Mozilla/5.0 (iPhone; CPU iPhone OS 15_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.0 Mobile/15E148 Safari/604.1",
            CreatedAt = SeederHelpers.GetRandomDateInPast(5, 15)
        },
        new Contact
        {
            FullName = "Hoàng Minh Quân",
            Email = SeederHelpers.GenerateRandomEmail(),
            Phone = SeederHelpers.GenerateRandomPhone(),
            Subject = "Thắc mắc về chính sách bảo hành sản phẩm Kova",
            Message = "Tôi mua sản phẩm Kova CT-11A tại cửa hàng của quý vị cách đây 3 tháng. Nay có một số vấn đề nhỏ, xin hỏi về chính sách bảo hành.",
            Status = ContactStatus.Resolved,
            AdminNotes = "Đã giải thích chính sách và hướng dẫn khách hàng gửi yêu cầu bảo hành chính thức.",
            IpAddress = "192.168.1.107",
            UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/14.1.2 Safari/605.1.15",
            CreatedAt = SeederHelpers.GetRandomDateInPast(10, 30)
        },
        new Contact
        {
            FullName = "Vũ Quang Vinh",
            Email = SeederHelpers.GenerateRandomEmail(),
            Phone = null, // No phone provided
            Subject = "Góp ý về website",
            Message = "Website của quý vị rất chuyên nghiệp, nhưng tôi thấy phần tìm kiếm sản phẩm đôi khi chưa chính xác lắm. Hy vọng có thể cải thiện trong tương lai.",
            Status = ContactStatus.Spam,
            AdminNotes = "Đã ghi nhận góp ý và chuyển cho bộ phận phát triển website.",
            IpAddress = "192.168.1.108",
            UserAgent = "Mozilla/5.0 (Linux; Android 11; Pixel 5) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Mobile Safari/537.36",
            CreatedAt = SeederHelpers.GetRandomDateInPast(30, 60)
        },
        new Contact
        {
            FullName = "Nguyễn Diệu Linh",
            Email = SeederHelpers.GenerateRandomEmail(),
            Phone = SeederHelpers.GenerateRandomPhone(),
            Subject = "Đặt mua số lượng lớn sơn lót",
            Message = "Tôi là đại diện nhà thầu XYZ, cần đặt 20 thùng sơn lót kháng kiềm Bestmix P901. Vui lòng liên hệ để trao đổi chi tiết về giá sỉ và vận chuyển.",
            Status = ContactStatus.New,
            IpAddress = "192.168.1.109",
            UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/92.0.4515.159 Safari/537.36",
            CreatedAt = SeederHelpers.GetRandomDateInPast(1, 5)
        }
    };

        await _dbContext.Contacts.AddRangeAsync(contacts);
        await _dbContext.SaveChangesAsync();
    }
}
