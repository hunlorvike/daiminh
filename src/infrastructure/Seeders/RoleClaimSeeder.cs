using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using shared.Constants;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class RoleClaimSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<RoleClaimSeeder> _logger;

    public RoleClaimSeeder(ApplicationDbContext dbContext, RoleManager<Role> roleManager, ILogger<RoleClaimSeeder> logger)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
        _logger = logger;
    }

    public int Order => 3;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding Role Claims...");

        var allClaimDefinitions = await _dbContext.ClaimDefinitions.ToListAsync();
        if (!allClaimDefinitions.Any())
        {
            _logger.LogWarning("No Claim Definitions found. Cannot assign claims to roles. Ensure ClaimDefinitionSeeder runs before this one.");
            return;
        }
        var claimDefDict = allClaimDefinitions.ToDictionary(cd => cd.Value);


        var adminRole = await _roleManager.FindByNameAsync("Admin");
        var superAdminRole = await _roleManager.FindByNameAsync("SuperAdmin");
        var userRole = await _roleManager.FindByNameAsync("User");

        if (superAdminRole != null)
        {
            await AssignClaimsToRole(superAdminRole, claimDefDict, new List<string>
            {
                PermissionConstants.SuperAdminAccess
            });
        }
        else
        {
            _logger.LogWarning("SuperAdmin role not found. Skipping claim assignment for SuperAdmin role.");
        }

        if (adminRole != null)
        {
            await AssignClaimsToRole(adminRole, claimDefDict, new List<string>
            {
                PermissionConstants.AdminAccess,
                PermissionConstants.DashboardView,

                PermissionConstants.FAQView, PermissionConstants.FAQCreate, PermissionConstants.FAQEdit, PermissionConstants.FAQDelete,
                PermissionConstants.ArticleView, PermissionConstants.ArticleCreate, PermissionConstants.ArticleEdit, PermissionConstants.ArticleDelete,
                PermissionConstants.BannerView, PermissionConstants.BannerCreate, PermissionConstants.BannerEdit, PermissionConstants.BannerDelete,
                PermissionConstants.BrandView, PermissionConstants.BrandCreate, PermissionConstants.BrandEdit, PermissionConstants.BrandDelete,
                PermissionConstants.CategoryView, PermissionConstants.CategoryCreate, PermissionConstants.CategoryEdit, PermissionConstants.CategoryDelete,
                PermissionConstants.ContactView, PermissionConstants.ContactCreate, PermissionConstants.ContactEdit, PermissionConstants.ContactDelete,
                PermissionConstants.NewsletterView, PermissionConstants.NewsletterCreate, PermissionConstants.NewsletterEdit, PermissionConstants.NewsletterDelete,
                PermissionConstants.PageView, PermissionConstants.PageCreate, PermissionConstants.PageEdit, PermissionConstants.PageDelete,
                PermissionConstants.PopupModalView, PermissionConstants.PopupModalCreate, PermissionConstants.PopupModalEdit, PermissionConstants.PopupModalDelete,
                PermissionConstants.ProductView, PermissionConstants.ProductCreate, PermissionConstants.ProductEdit, PermissionConstants.ProductDelete,
                PermissionConstants.ProductVariationView, PermissionConstants.ProductVariationCreate, PermissionConstants.ProductVariationEdit, PermissionConstants.ProductVariationDelete,
                PermissionConstants.TestimonialView, PermissionConstants.TestimonialCreate, PermissionConstants.TestimonialEdit, PermissionConstants.TestimonialDelete,
            });
        }
        else
        {
            _logger.LogWarning("Admin role not found. Skipping claim assignment for Admin role.");
        }

        _logger.LogInformation("Finished seeding Role Claims.");
    }

    private async Task AssignClaimsToRole(Role? role, Dictionary<string, ClaimDefinition> claimDefDict, List<string> desiredClaimValues)
    {
        if (role == null) return;

        var currentClaims = await _roleManager.GetClaimsAsync(role);

        var claimsToRemove = currentClaims
            .Where(c => c.Type == "Permission" && !desiredClaimValues.Contains(c.Value))
            .ToList();

        var claimsToAddValues = desiredClaimValues
            .Where(value => !currentClaims.Any(c => c.Type == "Permission" && c.Value == value))
            .ToList();

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