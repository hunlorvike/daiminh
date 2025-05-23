// infrastructure.Seeders/RoleClaimSeeder.cs

using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Thêm để dùng ToListAsync
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class RoleClaimSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext; // Vẫn cần để lấy ClaimDefinitions
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RoleClaimSeeder> _logger;

    public RoleClaimSeeder(ApplicationDbContext dbContext, RoleManager<Role> roleManager, ILogger<RoleClaimSeeder> logger)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
        _logger = logger;
    }

    public int Order => 3; // Đảm bảo chạy sau UserAndRoleSeeder

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding Role Claims...");

        // Lấy tất cả ClaimDefinitions (vẫn cần để biết các quyền hợp lệ)
        var allClaimDefinitions = await _dbContext.ClaimDefinitions.ToListAsync();
        if (!allClaimDefinitions.Any())
        {
            _logger.LogWarning("No Claim Definitions found. Cannot assign claims to roles. Ensure ClaimDefinitionSeeder runs before this one.");
            return;
        }
        // Tạo một Dictionary để dễ dàng tra cứu ClaimDefinition theo Value
        var claimDefDict = allClaimDefinitions.ToDictionary(cd => cd.Value);


        // Lấy các vai trò đã tạo
        var adminRole = await _roleManager.FindByNameAsync("Admin");
        var superAdminRole = await _roleManager.FindByNameAsync("SuperAdmin");
        var userRole = await _roleManager.FindByNameAsync("User");

        // Gán Claims cho Admin Role
        if (adminRole != null)
        {
            await AssignClaimsToRole(adminRole, claimDefDict, new List<string>
            {
                "Admin.Access",
                "Dashboard.View",

                "FAQ.View", "FAQ.Create", "FAQ.Edit", "FAQ.Delete",
                "Article.View", "Article.Create", "Article.Edit", "Article.Delete",
                "Attribute.View", "Attribute.Create", "Attribute.Edit", "Attribute.Delete",
                "Banner.View", "Banner.Create", "Banner.Edit", "Banner.Delete",
                "Category.View", "Category.Create", "Category.Edit", "Category.Delete",
                "Newsletter.View", "Newsletter.Create", "Newsletter.Edit", "Newsletter.Delete",
                "Page.View", "Page.Create", "Page.Edit", "Page.Delete",
                "PopupModal.View", "PopupModal.Create", "PopupModal.Edit", "PopupModal.Delete",
                "Product.View", "Product.Create", "Product.Edit", "Product.Delete",
                "ProductReview.View", "ProductReview.Edit", "ProductReview.Delete",
                "ProductVariation.View", "ProductVariation.Create", "ProductVariation.Edit", "ProductVariation.Delete",
                "Setting.Manage",
                "Slide.View", "Slide.Create", "Slide.Edit", "Slide.Delete",
                "Testimonial.View", "Testimonial.Create", "Testimonial.Edit", "Testimonial.Delete",
                "User.View", "User.Create", "User.Edit", "User.Delete", "User.ManageStatus", "User.ResetPassword",
                "Role.Manage",
                "ClaimDefinition.Manage",
            });
        }
        else
        {
            _logger.LogWarning("Admin role not found. Skipping claim assignment for Admin role.");
        }

        // Gán Claims cho SuperAdmin Role
        if (superAdminRole != null)
        {
            await AssignClaimsToRole(superAdminRole, claimDefDict, new List<string>
            {
                "SuperAdmin.Access"
            });
        }
        else
        {
            _logger.LogWarning("SuperAdmin role not found. Skipping claim assignment for SuperAdmin role.");
        }

        // Gán Claims cho User Role
        if (userRole != null)
        {
            await AssignClaimsToRole(userRole, claimDefDict, new List<string>
            {
                "Product.View"
            });
        }
        else
        {
            _logger.LogWarning("User role not found. Skipping claim assignment for User role.");
        }

        _logger.LogInformation("Finished seeding Role Claims.");
    }

    // Phương thức để gán/đồng bộ claims cho một vai trò (ĐÃ SỬA ĐỔI)
    private async Task AssignClaimsToRole(Role? role, Dictionary<string, ClaimDefinition> claimDefDict, List<string> desiredClaimValues)
    {
        if (role == null) return;

        // Lấy tất cả claims hiện tại của vai trò này thông qua RoleManager
        var currentClaims = await _roleManager.GetClaimsAsync(role);

        // Xác định các claims cần xóa
        var claimsToRemove = currentClaims
            .Where(c => c.Type == "Permission" && !desiredClaimValues.Contains(c.Value))
            .ToList();

        // Xác định các claim values cần thêm
        var claimsToAddValues = desiredClaimValues
            .Where(value => !currentClaims.Any(c => c.Type == "Permission" && c.Value == value))
            .ToList();

        // Xóa các claims không còn mong muốn
        foreach (var claim in claimsToRemove)
        {
            var result = await _roleManager.RemoveClaimAsync(role, claim);
            if (result.Succeeded)
            {
                _logger.LogInformation("Removed claim '{ClaimType}:{ClaimValue}' from role '{RoleName}'.", claim.Type, claim.Value, role.Name);
            }
            else
            {
                _logger.LogError("Failed to remove claim '{ClaimType}:{ClaimValue}' from role '{RoleName}': {Errors}", claim.Type, claim.Value, role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        foreach (var claimValue in claimsToAddValues)
        {
            if (claimDefDict.TryGetValue(claimValue, out var claimDef) && claimDef.Type == "Permission")
            {
                var newClaim = new System.Security.Claims.Claim(claimDef.Type, claimDef.Value);
                var result = await _roleManager.AddClaimAsync(role, newClaim);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Added claim '{ClaimType}:{ClaimValue}' to role '{RoleName}'.", newClaim.Type, newClaim.Value, role.Name);
                }
                else
                {
                    _logger.LogError("Failed to add claim '{ClaimType}:{ClaimValue}' to role '{RoleName}': {Errors}", newClaim.Type, newClaim.Value, role.Name, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                _logger.LogWarning("Claim Definition for value '{ClaimValue}' with type 'Permission' not found. Cannot add to role '{RoleName}'.", claimValue, role.Name);
            }
        }
    }
}