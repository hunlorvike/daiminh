using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Enums;
using shared.Helpers;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class CategorySeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public CategorySeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 5;
    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Categories...");

        if (await _dbContext.Categories.AnyAsync())
        {
            return;
        }

        // Product Categories (Level 1)
        var productCategories = new List<Category>
        {
            new Category { Name = "Sơn Nội Thất", Slug = SlugHelper.Generate("Sơn Nội Thất"), Description = "Các loại sơn dùng cho không gian bên trong nhà, mang lại vẻ đẹp và sự thoải mái.", Type = CategoryType.Product, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Ngoại Thất", Slug = SlugHelper.Generate("Sơn Ngoại Thất"), Description = "Các loại sơn chuyên dụng bảo vệ và trang trí bên ngoài công trình, chịu được thời tiết khắc nghiệt.", Type = CategoryType.Product, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Lót & Chống Kiềm", Slug = SlugHelper.Generate("Sơn Lót & Chống Kiềm"), Description = "Sơn lót giúp tăng cường độ bám dính và chống lại tác động của kiềm từ vật liệu xây dựng.", Type = CategoryType.Product, OrderIndex = 3, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Chống Thấm", Slug = SlugHelper.Generate("Sơn Chống Thấm"), Description = "Các sản phẩm chuyên biệt dùng để ngăn chặn sự xâm nhập của nước vào công trình.", Type = CategoryType.Product, OrderIndex = 4, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Vật Liệu Chống Thấm", Slug = SlugHelper.Generate("Vật Liệu Chống Thấm"), Description = "Màng, hóa chất, phụ gia chuyên dụng cho công tác chống thấm.", Type = CategoryType.Product, OrderIndex = 5, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Dụng Cụ Sơn", Slug = SlugHelper.Generate("Dụng Cụ Sơn"), Description = "Các công cụ, thiết bị hỗ trợ quá trình thi công sơn.", Type = CategoryType.Product, OrderIndex = 6, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Công Nghiệp", Slug = SlugHelper.Generate("Sơn Công Nghiệp"), Description = "Sơn chuyên dụng cho các bề mặt kim loại, sàn nhà xưởng, tàu biển, etc.", Type = CategoryType.Product, OrderIndex = 7, IsActive = true, CreatedAt = DateTime.Now }
        };
        await _dbContext.Categories.AddRangeAsync(productCategories);
        await _dbContext.SaveChangesAsync(); // Save to get IDs

        var sonNoiThatCat = productCategories.First(c => c.Slug == SlugHelper.Generate("Sơn Nội Thất"));
        var sonNgoaiThatCat = productCategories.First(c => c.Slug == SlugHelper.Generate("Sơn Ngoai Thất"));
        var sonChongThamCat = productCategories.First(c => c.Slug == SlugHelper.Generate("Sơn Chống Thấm"));
        var vatLieuChongThamCat = productCategories.First(c => c.Slug == SlugHelper.Generate("Vật Liệu Chống Thấm"));

        // Product Categories (Level 2)
        var subProductCategories = new List<Category>
        {
            new Category { Name = "Sơn Bóng Nội Thất", Slug = SlugHelper.Generate("Sơn Bóng Nội Thất"), Description = "Sơn nội thất có độ bóng cao, dễ lau chùi.", Type = CategoryType.Product, ParentId = sonNoiThatCat.Id, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Mờ Nội Thất", Slug = SlugHelper.Generate("Sơn Mờ Nội Thất"), Description = "Sơn nội thất có độ mờ, mang lại vẻ đẹp sang trọng.", Type = CategoryType.Product, ParentId = sonNoiThatCat.Id, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Chống Thấm Tường Đứng", Slug = SlugHelper.Generate("Sơn Chống Thấm Tường Đứng"), Description = "Sản phẩm chống thấm cho bề mặt tường đứng.", Type = CategoryType.Product, ParentId = sonChongThamCat.Id, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Chống Thấm Sàn Mái", Slug = SlugHelper.Generate("Sơn Chống Thấm Sàn Mái"), Description = "Sản phẩm chống thấm cho sàn mái, sân thượng.", Type = CategoryType.Product, ParentId = sonChongThamCat.Id, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Màng Chống Thấm", Slug = SlugHelper.Generate("Màng Chống Thấm"), Description = "Các loại màng bitum, HDPE dùng trong chống thấm.", Type = CategoryType.Product, ParentId = vatLieuChongThamCat.Id, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Phụ Gia Chống Thấm", Slug = SlugHelper.Generate("Phụ Gia Chống Thấm"), Description = "Các chất phụ gia tăng cường khả năng chống thấm cho vữa, bê tông.", Type = CategoryType.Product, ParentId = vatLieuChongThamCat.Id, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Sơn Cao Cấp Ngoại Thất", Slug = SlugHelper.Generate("Sơn Cao Cấp Ngoại Thất"), Description = "Sơn ngoại thất với độ bền và màu sắc vượt trội.", Type = CategoryType.Product, ParentId = sonNgoaiThatCat.Id, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.Now },
        };
        await _dbContext.Categories.AddRangeAsync(subProductCategories);
        await _dbContext.SaveChangesAsync(); // Save to get IDs

        // Article Categories
        var articleCategories = new List<Category>
        {
            new Category { Name = "Hướng Dẫn Thi Công", Slug = SlugHelper.Generate("Hướng Dẫn Thi Công"), Description = "Các bài viết hướng dẫn chi tiết cách thi công sơn và chống thấm.", Type = CategoryType.Article, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Kinh Nghiệm Sơn", Slug = SlugHelper.Generate("Kinh Nghiệm Sơn"), Description = "Chia sẻ kinh nghiệm thực tế khi chọn và sử dụng sơn.", Type = CategoryType.Article, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Dự Án Tiêu Biểu", Slug = SlugHelper.Generate("Dự Án Tiêu Biểu"), Description = "Giới thiệu các dự án đã hoàn thành, ứng dụng sản phẩm của chúng tôi.", Type = CategoryType.Article, OrderIndex = 3, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Tin Tức & Sự Kiện", Slug = SlugHelper.Generate("Tin Tức & Sự Kiện"), Description = "Cập nhật các tin tức mới nhất về ngành sơn và chống thấm, các sự kiện của công ty.", Type = CategoryType.Article, OrderIndex = 4, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Kiến Thức Chung", Slug = SlugHelper.Generate("Kiến Thức Chung"), Description = "Tổng hợp kiến thức cơ bản về sơn, chống thấm, vật liệu xây dựng.", Type = CategoryType.Article, OrderIndex = 5, IsActive = true, CreatedAt = DateTime.Now }
        };
        await _dbContext.Categories.AddRangeAsync(articleCategories);
        await _dbContext.SaveChangesAsync(); // Save to get IDs

        // FAQ Categories
        var faqCategories = new List<Category>
        {
            new Category { Name = "Câu Hỏi Về Sản Phẩm", Slug = SlugHelper.Generate("Câu Hỏi Về Sản Phẩm"), Description = "Các câu hỏi thường gặp về tính năng và ứng dụng sản phẩm.", Type = CategoryType.FAQ, OrderIndex = 1, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Câu Hỏi Về Dịch Vụ", Slug = SlugHelper.Generate("Câu Hỏi Về Dịch Vụ"), Description = "Các thắc mắc liên quan đến dịch vụ tư vấn, thi công, bảo hành.", Type = CategoryType.FAQ, OrderIndex = 2, IsActive = true, CreatedAt = DateTime.Now },
            new Category { Name = "Câu Hỏi Về Đặt Hàng", Slug = SlugHelper.Generate("Câu Hỏi Về Đặt Hàng"), Description = "Các vấn đề về quy trình đặt hàng, thanh toán, vận chuyển.", Type = CategoryType.FAQ, OrderIndex = 3, IsActive = true, CreatedAt = DateTime.Now }
        };
        await _dbContext.Categories.AddRangeAsync(faqCategories);
        await _dbContext.SaveChangesAsync();
    }
}