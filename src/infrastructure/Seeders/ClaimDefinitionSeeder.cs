using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class ClaimDefinitionSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;

    public ClaimDefinitionSeeder(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public int Order => 1;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Claim Definitions...");

        if (await _dbContext.ClaimDefinitions.AnyAsync())
        {
            Console.WriteLine("Claim Definitions already exist. Skipping seeding.");
            return;
        }

        var claims = new List<ClaimDefinition>
        {
            new ClaimDefinition { Type = "Permission", Value = "Admin.Access", Description = "Quyền truy cập khu vực quản trị" },

            new ClaimDefinition { Type = "Permission", Value = "Article.View", Description = "Xem danh sách bài viết" },
            new ClaimDefinition { Type = "Permission", Value = "Article.Create", Description = "Tạo bài viết mới" },
            new ClaimDefinition { Type = "Permission", Value = "Article.Edit", Description = "Chỉnh sửa bài viết" },
            new ClaimDefinition { Type = "Permission", Value = "Article.Delete", Description = "Xóa bài viết" },

            new ClaimDefinition { Type = "Permission", Value = "Attribute.View", Description = "Xem danh sách thuộc tính sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "Attribute.Create", Description = "Tạo thuộc tính sản phẩm mới" },
            new ClaimDefinition { Type = "Permission", Value = "Attribute.Edit", Description = "Chỉnh sửa thuộc tính sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "Attribute.Delete", Description = "Xóa thuộc tính sản phẩm" },

            new ClaimDefinition { Type = "Permission", Value = "Banner.View", Description = "Xem danh sách banner" },
            new ClaimDefinition { Type = "Permission", Value = "Banner.Create", Description = "Tạo banner mới" },
            new ClaimDefinition { Type = "Permission", Value = "Banner.Edit", Description = "Chỉnh sửa banner" },
            new ClaimDefinition { Type = "Permission", Value = "Banner.Delete", Description = "Xóa banner" },

            new ClaimDefinition { Type = "Permission", Value = "Category.View", Description = "Xem danh mục" },
            new ClaimDefinition { Type = "Permission", Value = "Category.Create", Description = "Tạo danh mục mới" },
            new ClaimDefinition { Type = "Permission", Value = "Category.Edit", Description = "Chỉnh sửa danh mục" },
            new ClaimDefinition { Type = "Permission", Value = "Category.Delete", Description = "Xóa danh mục" },

            new ClaimDefinition { Type = "Permission", Value = "FAQ.View", Description = "Xem danh sách FAQ" },
            new ClaimDefinition { Type = "Permission", Value = "FAQ.Create", Description = "Tạo FAQ mới" },
            new ClaimDefinition { Type = "Permission", Value = "FAQ.Edit", Description = "Chỉnh sửa FAQ" },
            new ClaimDefinition { Type = "Permission", Value = "FAQ.Delete", Description = "Xóa FAQ" },

            new ClaimDefinition { Type = "Permission", Value = "Newsletter.View", Description = "Xem danh sách đăng ký nhận tin" },
            new ClaimDefinition { Type = "Permission", Value = "Newsletter.Create", Description = "Tạo đăng ký nhận tin mới" },
            new ClaimDefinition { Type = "Permission", Value = "Newsletter.Edit", Description = "Chỉnh sửa đăng ký nhận tin" },
            new ClaimDefinition { Type = "Permission", Value = "Newsletter.Delete", Description = "Xóa đăng ký nhận tin" },

            new ClaimDefinition { Type = "Permission", Value = "Page.View", Description = "Xem danh sách trang tĩnh" },
            new ClaimDefinition { Type = "Permission", Value = "Page.Create", Description = "Tạo trang tĩnh mới" },
            new ClaimDefinition { Type = "Permission", Value = "Page.Edit", Description = "Chỉnh sửa trang tĩnh" },
            new ClaimDefinition { Type = "Permission", Value = "Page.Delete", Description = "Xóa trang tĩnh" },

            new ClaimDefinition { Type = "Permission", Value = "PopupModal.View", Description = "Xem danh sách Popup" },
            new ClaimDefinition { Type = "Permission", Value = "PopupModal.Create", Description = "Tạo Popup mới" },
            new ClaimDefinition { Type = "Permission", Value = "PopupModal.Edit", Description = "Chỉnh sửa Popup" },
            new ClaimDefinition { Type = "Permission", Value = "PopupModal.Delete", Description = "Xóa Popup" },

            new ClaimDefinition { Type = "Permission", Value = "Product.View", Description = "Xem danh sách sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "Product.Create", Description = "Tạo sản phẩm mới" },
            new ClaimDefinition { Type = "Permission", Value = "Product.Edit", Description = "Chỉnh sửa sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "Product.Delete", Description = "Xóa sản phẩm" },

            new ClaimDefinition { Type = "Permission", Value = "ProductReview.View", Description = "Xem danh sách đánh giá sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "ProductReview.Edit", Description = "Cập nhật trạng thái đánh giá sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "ProductReview.Delete", Description = "Xóa đánh giá sản phẩm" },

            new ClaimDefinition { Type = "Permission", Value = "ProductVariation.View", Description = "Xem danh sách biến thể sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "ProductVariation.Create", Description = "Tạo biến thể sản phẩm mới" },
            new ClaimDefinition { Type = "Permission", Value = "ProductVariation.Edit", Description = "Chỉnh sửa biến thể sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "ProductVariation.Delete", Description = "Xóa biến thể sản phẩm" },

            new ClaimDefinition { Type = "Permission", Value = "Role.Manage", Description = "Quản lý vai trò (CRUD)" },

            new ClaimDefinition { Type = "Permission", Value = "ClaimDefinition.Manage", Description = "Quản lý định nghĩa quyền hạn (CRUD)" },

            new ClaimDefinition { Type = "Permission", Value = "Setting.Manage", Description = "Quản lý cài đặt hệ thống (Xem & Sửa)" },

            new ClaimDefinition { Type = "Permission", Value = "Slide.View", Description = "Xem danh sách slide" },
            new ClaimDefinition { Type = "Permission", Value = "Slide.Create", Description = "Tạo slide mới" },
            new ClaimDefinition { Type = "Permission", Value = "Slide.Edit", Description = "Chỉnh sửa slide" },
            new ClaimDefinition { Type = "Permission", Value = "Slide.Delete", Description = "Xóa slide" },

            new ClaimDefinition { Type = "Permission", Value = "Testimonial.View", Description = "Xem danh sách đánh giá khách hàng" },
            new ClaimDefinition { Type = "Permission", Value = "Testimonial.Create", Description = "Tạo đánh giá khách hàng mới" },
            new ClaimDefinition { Type = "Permission", Value = "Testimonial.Edit", Description = "Chỉnh sửa đánh giá khách hàng" },
            new ClaimDefinition { Type = "Permission", Value = "Testimonial.Delete", Description = "Xóa đánh giá khách hàng" },

            new ClaimDefinition { Type = "Permission", Value = "User.Manage", Description = "Quản lý người dùng (Xem & Sửa)" },
        };

        await _dbContext.ClaimDefinitions.AddRangeAsync(claims);
        await _dbContext.SaveChangesAsync();

        Console.WriteLine("Claim Definitions seeded successfully.");
    }
}