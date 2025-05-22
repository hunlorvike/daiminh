using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class BrandSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public BrandSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 4;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Brands...");

        if (await _dbContext.Brands.AnyAsync()) return;

        var brands = new List<Brand>
        {
            new Brand
            {
                Name = "Kova",
                Slug = "kova",
                Description = "Thương hiệu sơn Việt Nam nổi tiếng với công nghệ nano chống thấm, phù hợp khí hậu nhiệt đới.",
                LogoUrl = "/img/brands/kova.jpg",
                Website = "https://kova.com.vn",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Brand
            {
                Name = "Dulux",
                Slug = "dulux",
                Description = "Thương hiệu toàn cầu thuộc AkzoNobel, nổi bật với sản phẩm sơn nội thất và ngoại thất cao cấp.",
                LogoUrl = "/img/brands/dulux.png",
                Website = "https://www.dulux.com.vn",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Brand
            {
                Name = "Jotun",
                Slug = "jotun",
                Description = "Tập đoàn Na Uy chuyên về sơn công nghiệp, sơn hàng hải và sơn trang trí.",
                LogoUrl = "/img/brands/jotun.png",
                Website = "https://www.jotun.com/vn",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Brand
            {
                Name = "Mykolor",
                Slug = "mykolor",
                Description = "Dòng sơn cao cấp của 4 Oranges, nổi bật với bảng màu thời trang, độ bền màu cao.",
                LogoUrl = "/img/brands/mykolor.jpg",
                Website = "https://www.mykolor.com.vn",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Brand
            {
                Name = "Nippon Paint",
                Slug = "nippon-paint",
                Description = "Thương hiệu Nhật Bản với nhiều sản phẩm sơn chất lượng, thân thiện môi trường.",
                LogoUrl = "/img/brands/nippon.png",
                Website = "https://www.nipponpaint.com.vn",
                IsActive = true,
                CreatedAt = DateTime.Now
            },
            new Brand
            {
                Name = "Maxilite",
                Slug = "maxilite",
                Description = "Thương hiệu phổ thông của AkzoNobel, cung cấp sơn nước chất lượng với giá hợp lý.",
                LogoUrl = "/img/brands/maxilite.jpg",
                Website = "https://www.dulux.com.vn/vi/maxilite",
                IsActive = true,
                CreatedAt = DateTime.Now
            }
        };

        await _dbContext.Brands.AddRangeAsync(brands);
        await _dbContext.SaveChangesAsync();
    }
}
