using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Enums;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class TagSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public TagSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 6;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Tags...");

        if (await _dbContext.Tags.AnyAsync())
        {
            return;
        }

        var tags = new List<Tag>
        {
            // Product Tags
            new Tag { Name = "Sơn cao cấp", Slug = SlugHelper.Generate("Sơn cao cấp"), Type = TagType.Product, Description = "Các sản phẩm sơn có chất lượng và độ bền vượt trội." , CreatedAt = DateTime.Now },
            new Tag { Name = "Chống thấm dột", Slug = SlugHelper.Generate("Chống thấm dột"), Type = TagType.Product, Description = "Các sản phẩm chuyên dùng để xử lý và ngăn ngừa thấm dột." , CreatedAt = DateTime.Now },
            new Tag { Name = "Bền màu", Slug = SlugHelper.Generate("Bền màu"), Type = TagType.Product, Description = "Sản phẩm sơn giữ màu lâu theo thời gian." , CreatedAt = DateTime.Now },
            new Tag { Name = "Dễ lau chùi", Slug = SlugHelper.Generate("Dễ lau chùi"), Type = TagType.Product, Description = "Sơn dễ dàng vệ sinh các vết bẩn trên bề mặt." , CreatedAt = DateTime.Now },
            new Tag { Name = "Chống nấm mốc", Slug = SlugHelper.Generate("Chống nấm mốc"), Type = TagType.Product, Description = "Sơn có khả năng ngăn chặn sự phát triển của nấm mốc." , CreatedAt = DateTime.Now },
            new Tag { Name = "Thân thiện môi trường", Slug = SlugHelper.Generate("Thân thiện môi trường"), Type = TagType.Product, Description = "Sản phẩm ít độc hại, an toàn cho môi trường và người dùng." , CreatedAt = DateTime.Now },

            // Article Tags
            new Tag { Name = "Mẹo thi công", Slug = SlugHelper.Generate("Mẹo thi công"), Type = TagType.Article, Description = "Những mẹo nhỏ giúp quá trình thi công dễ dàng và hiệu quả." , CreatedAt = DateTime.Now },
            new Tag { Name = "Tư vấn chọn sơn", Slug = SlugHelper.Generate("Tư vấn chọn sơn"), Type = TagType.Article, Description = "Bài viết hướng dẫn cách lựa chọn loại sơn phù hợp." , CreatedAt = DateTime.Now },
            new Tag { Name = "Xử lý thấm dột", Slug = SlugHelper.Generate("Xử lý thấm dột"), Type = TagType.Article, Description = "Các phương pháp và sản phẩm để xử lý triệt để tình trạng thấm dột." , CreatedAt = DateTime.Now },
            new Tag { Name = "Bảo dưỡng nhà cửa", Slug = SlugHelper.Generate("Bảo dưỡng nhà cửa"), Type = TagType.Article, Description = "Lời khuyên để duy trì và bảo dưỡng ngôi nhà luôn mới đẹp." , CreatedAt = DateTime.Now },
            new Tag { Name = "Công nghệ sơn mới", Slug = SlugHelper.Generate("Công nghệ sơn mới"), Type = TagType.Article, Description = "Cập nhật các công nghệ và xu hướng mới nhất trong ngành sơn." , CreatedAt = DateTime.Now },

            // General Tags
            new Tag { Name = "Khuyến mãi", Slug = SlugHelper.Generate("Khuyến mãi"), Type = TagType.General, Description = "Các chương trình ưu đãi, giảm giá sản phẩm." , CreatedAt = DateTime.Now },
            new Tag { Name = "Sản phẩm mới", Slug = SlugHelper.Generate("Sản phẩm mới"), Type = TagType.General, Description = "Giới thiệu các sản phẩm vừa ra mắt trên thị trường." , CreatedAt = DateTime.Now },
            new Tag { Name = "Bảo hành", Slug = SlugHelper.Generate("Bảo hành"), Type = TagType.General, Description = "Thông tin liên quan đến chính sách bảo hành sản phẩm." , CreatedAt = DateTime.Now }
        };

        await _dbContext.Tags.AddRangeAsync(tags);
        await _dbContext.SaveChangesAsync();
    }
}

