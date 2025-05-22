using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class FAQSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public FAQSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 8;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding FAQs...");

        if (await _dbContext.FAQs.AnyAsync())
        {
            return; // Already seeded
        }

        var spCat = await _dbContext.Categories.FirstAsync(c => c.Slug == SlugHelper.Generate("Câu Hỏi Về Sản Phẩm"));
        var dvCat = await _dbContext.Categories.FirstAsync(c => c.Slug == SlugHelper.Generate("Câu Hỏi Về Dịch Vụ"));
        var dhCat = await _dbContext.Categories.FirstAsync(c => c.Slug == SlugHelper.Generate("Câu Hỏi Về Đặt Hàng"));

        var faqs = new List<FAQ>
        {
            new FAQ
            {
                Question = "Sơn chống thấm Kova CT-11A có cần pha thêm nước không?",
                Answer = "Sơn chống thấm Kova CT-11A thường không cần pha thêm nước. Tuy nhiên, trong một số trường hợp cụ thể (như lớp lót đầu tiên trên bề mặt hút nước mạnh), nhà sản xuất có thể khuyến nghị pha loãng một lượng rất nhỏ. Luôn đọc kỹ hướng dẫn sử dụng trên bao bì sản phẩm.",
                OrderIndex = 1, IsActive = true, CategoryId = spCat.Id, CreatedAt = DateTime.Now
            },
            new FAQ
            {
                Question = "Sơn nội thất Dulux 5in1 có thể dùng cho ngoại thất không?",
                Answer = "Không, sơn nội thất Dulux 5in1 được thiết kế chuyên biệt cho các bề mặt trong nhà. Đối với ngoại thất, bạn nên sử dụng các dòng sản phẩm sơn ngoại thất chuyên dụng như Dulux Weathershield để đảm bảo độ bền và khả năng chống chịu thời tiết.",
                OrderIndex = 2, IsActive = true, CategoryId = spCat.Id, CreatedAt = DateTime.Now
            },
            new FAQ
            {
                Question = "Thời gian bảo hành cho công trình chống thấm là bao lâu?",
                Answer = "Thời gian bảo hành cho công trình chống thấm tùy thuộc vào loại vật liệu, hạng mục thi công và thỏa thuận trong hợp đồng. Thông thường, thời gian bảo hành dao động từ 2 đến 10 năm. Vui lòng liên hệ bộ phận chăm sóc khách hàng để được tư vấn chi tiết về từng gói dịch vụ.",
                OrderIndex = 1, IsActive = true, CategoryId = dvCat.Id, CreatedAt = DateTime.Now
            },
            new FAQ
            {
                Question = "Làm thế nào để yêu cầu báo giá thi công?",
                Answer = "Bạn có thể yêu cầu báo giá thi công bằng cách điền form liên hệ trên website, gọi điện trực tiếp đến hotline, hoặc gửi email cho chúng tôi. Vui lòng cung cấp đầy đủ thông tin về hạng mục, diện tích và yêu cầu cụ thể để chúng tôi có thể đưa ra báo giá chính xác nhất.",
                OrderIndex = 2, IsActive = true, CategoryId = dvCat.Id, CreatedAt = DateTime.Now
            },
            new FAQ
            {
                Question = "Tôi có thể thanh toán bằng những hình thức nào?",
                Answer = "Chúng tôi chấp nhận nhiều hình thức thanh toán đa dạng như chuyển khoản ngân hàng, thanh toán tiền mặt khi nhận hàng (COD), hoặc qua các ví điện tử phổ biến. Chi tiết về hình thức thanh toán sẽ được xác nhận khi bạn đặt hàng.",
                OrderIndex = 1, IsActive = true, CategoryId = dhCat.Id, CreatedAt = DateTime.Now
            },
            new FAQ
            {
                Question = "Thời gian giao hàng là bao lâu?",
                Answer = "Thời gian giao hàng thông thường là từ 1-3 ngày làm việc đối với các khu vực nội thành. Đối với các tỉnh/thành phố khác, thời gian giao hàng có thể lâu hơn tùy thuộc vào địa điểm. Chúng tôi sẽ thông báo thời gian giao hàng cụ thể khi xác nhận đơn hàng của bạn.",
                OrderIndex = 2, IsActive = true, CategoryId = dhCat.Id, CreatedAt = DateTime.Now
            },
            new FAQ
            {
                Question = "Sơn lót có thực sự cần thiết khi sơn nhà không?",
                Answer = "Sơn lót rất cần thiết. Nó giúp tăng cường độ bám dính của lớp sơn phủ, chống kiềm hóa, và giúp màu sơn lên đều, đẹp hơn, đồng thời kéo dài tuổi thọ cho lớp sơn hoàn thiện.",
                OrderIndex = 3, IsActive = true, CategoryId = spCat.Id, CreatedAt = DateTime.Now
            },
            new FAQ
            {
                Question = "Làm thế nào để chọn đúng loại sơn cho từng khu vực trong nhà?",
                Answer = "Mỗi khu vực trong nhà có đặc điểm và yêu cầu riêng. Ví dụ, phòng khách cần sơn bền đẹp, dễ lau chùi; phòng ngủ cần sơn an toàn, mùi nhẹ; phòng tắm, bếp cần sơn chống nấm mốc, chống thấm. Bạn nên tham khảo ý kiến chuyên gia hoặc đọc kỹ thông tin sản phẩm để chọn lựa phù hợp.",
                OrderIndex = 4, IsActive = true, CategoryId = spCat.Id, CreatedAt = DateTime.Now
            }
        };

        await _dbContext.FAQs.AddRangeAsync(faqs);
        await _dbContext.SaveChangesAsync();
    }
}
