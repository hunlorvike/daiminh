using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using shared.Constants;

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

        var allPermissions = PermissionConstants.GetAllPermissions().ToList();

        var claims = new List<ClaimDefinition>();
        foreach (var permissionValue in allPermissions)
        {
            claims.Add(new ClaimDefinition
            {
                Type = "Permission",
                Value = permissionValue,
                Description = ""
            });
        }

        await _dbContext.ClaimDefinitions.AddRangeAsync(claims);
        await _dbContext.SaveChangesAsync();

        Console.WriteLine("Claim Definitions seeded successfully.");
    }
}