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

    public int Order => 2;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Claim Definitions...");

        if (await _dbContext.ClaimDefinitions.AnyAsync())
        {
            return;
        }

        var claims = new List<ClaimDefinition>
        {
            new ClaimDefinition { Type = "Permission", Value = "Product.View", Description = "Xem sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "Product.Create", Description = "Tạo sản phẩm mới" },
            new ClaimDefinition { Type = "Permission", Value = "Product.Edit", Description = "Chỉnh sửa sản phẩm" },
            new ClaimDefinition { Type = "Permission", Value = "Product.Delete", Description = "Xóa sản phẩm" },

            new ClaimDefinition { Type = "Permission", Value = "Category.View", Description = "Xem danh mục" },
            new ClaimDefinition { Type = "Permission", Value = "Category.Create", Description = "Tạo danh mục mới" },
            new ClaimDefinition { Type = "Permission", Value = "Category.Edit", Description = "Chỉnh sửa danh mục" },
            new ClaimDefinition { Type = "Permission", Value = "Category.Delete", Description = "Xóa danh mục" },

            new ClaimDefinition { Type = "Permission", Value = "Article.View", Description = "Xem bài viết" },
            new ClaimDefinition { Type = "Permission", Value = "Article.Create", Description = "Tạo bài viết mới" },
            new ClaimDefinition { Type = "Permission", Value = "Article.Edit", Description = "Chỉnh sửa bài viết" },
            new ClaimDefinition { Type = "Permission", Value = "Article.Delete", Description = "Xóa bài viết" },

            new ClaimDefinition { Type = "Permission", Value = "User.View", Description = "Xem người dùng" },
            new ClaimDefinition { Type = "Permission", Value = "User.Create", Description = "Tạo người dùng mới" },
            new ClaimDefinition { Type = "Permission", Value = "User.Edit", Description = "Chỉnh sửa người dùng" },
            new ClaimDefinition { Type = "Permission", Value = "User.Delete", Description = "Xóa người dùng" },

            new ClaimDefinition { Type = "Permission", Value = "Role.View", Description = "Xem vai trò" },
            new ClaimDefinition { Type = "Permission", Value = "Role.Create", Description = "Tạo vai trò mới" },
            new ClaimDefinition { Type = "Permission", Value = "Role.Edit", Description = "Chỉnh sửa vai trò" },
            new ClaimDefinition { Type = "Permission", Value = "Role.Delete", Description = "Xóa vai trò" },

            new ClaimDefinition { Type = "Permission", Value = "Setting.View", Description = "Xem cài đặt hệ thống" },
            new ClaimDefinition { Type = "Permission", Value = "Setting.Edit", Description = "Chỉnh sửa cài đặt hệ thống" },

            new ClaimDefinition { Type = "Permission", Value = "Dashboard.View", Description = "Xem bảng điều khiển" },
            new ClaimDefinition { Type = "Permission", Value = "All.Manage", Description = "Quyền quản lý toàn bộ hệ thống" }
        };

        await _dbContext.ClaimDefinitions.AddRangeAsync(claims);
        await _dbContext.SaveChangesAsync();
    }
}
