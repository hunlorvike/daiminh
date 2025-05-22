using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class UserAndRoleSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ILogger<UserAndRoleSeeder> _logger;

    public UserAndRoleSeeder(ApplicationDbContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager, ILogger<UserAndRoleSeeder> logger)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
    }

    public int Order => 1;

    public async Task SeedAsync()
    {
        _logger.LogInformation("Seeding Users and Roles...");

        // 1. Seed Roles
        await CreateRoleIfNotExists("Admin", "ADMIN");
        await CreateRoleIfNotExists("User", "USER");

        // 2. Seed Admin User
        await CreateUserAndAssignRole("admin", "admin@admin.com", "Quản trị viên", "Password123!", "Admin");

        // 3. Seed Regular Users
        await CreateUserAndAssignRole("user1", "user1@example.com", "Nguyễn Văn A", "Password123!", "User");
        await CreateUserAndAssignRole("user2", "user2@example.com", "Trần Thị B", "Password123!", "User");

        _logger.LogInformation("Finished seeding Users and Roles.");
    }

    private async Task CreateRoleIfNotExists(string roleName, string normalizedRoleName)
    {
        var role = await _roleManager.FindByNameAsync(roleName);
        if (role == null)
        {
            role = new Role { Name = roleName, NormalizedName = normalizedRoleName, ConcurrencyStamp = Guid.NewGuid().ToString() };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                _logger.LogInformation("Created role: {RoleName}", roleName);
            }
            else
            {
                _logger.LogError("Failed to create role {RoleName}: {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            _logger.LogInformation("Role {RoleName} already exists.", roleName);
        }
    }

    private async Task CreateUserAndAssignRole(string userName, string email, string fullName, string password, string roleName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user == null)
        {
            user = new User
            {
                UserName = userName,
                Email = email,
                FullName = fullName,
                EmailConfirmed = true,
                IsActive = true,
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("Created user: {UserName}", userName);
                if (!string.IsNullOrEmpty(roleName))
                {
                    var roleExists = await _roleManager.RoleExistsAsync(roleName);
                    if (roleExists)
                    {
                        var roleAssignmentResult = await _userManager.AddToRoleAsync(user, roleName);
                        if (roleAssignmentResult.Succeeded)
                        {
                            _logger.LogInformation("Assigned role '{RoleName}' to user '{UserName}'.", roleName, userName);
                        }
                        else
                        {
                            _logger.LogError("Failed to assign role '{RoleName}' to user '{UserName}': {Errors}", roleName, userName, string.Join(", ", roleAssignmentResult.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Role '{RoleName}' does not exist for user '{UserName}'.", roleName, userName);
                    }
                }
            }
            else
            {
                _logger.LogError("Failed to create user {UserName}: {Errors}", userName, string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            _logger.LogInformation("User {UserName} already exists.", userName);
            if (!string.IsNullOrEmpty(roleName) && !await _userManager.IsInRoleAsync(user, roleName))
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (roleExists)
                {
                    var result = await _userManager.AddToRoleAsync(user, roleName);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Assigned missing role '{RoleName}' to existing user '{UserName}'.", roleName, userName);
                    }
                    else
                    {
                        _logger.LogError("Failed to assign missing role '{RoleName}' to existing user '{UserName}': {Errors}", roleName, userName, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    _logger.LogWarning("Role '{RoleName}' does not exist for existing user '{UserName}'.", roleName, userName);
                }
            }
        }
    }
}