using AutoRegister;
using domain.Entities;
using infrastructure.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure.Seeders;

[Register(ServiceLifetime.Transient)]
public class RoleClaimSeeder : IDataSeeder
{
    private readonly ApplicationDbContext _dbContext;
    private readonly RoleManager<Role> _roleManager;

    public RoleClaimSeeder(ApplicationDbContext dbContext, RoleManager<Role> roleManager)
    {
        _dbContext = dbContext;
        _roleManager = roleManager;
    }

    public int Order => 3;

    public async Task SeedAsync()
    {
        Console.WriteLine("Seeding Role Claims...");

        var adminRole = await _roleManager.FindByNameAsync("Admin");
        if (adminRole == null) return;

        var allClaims = await _dbContext.ClaimDefinitions.ToListAsync();
        var adminClaims = new List<RoleClaim>();

        foreach (var claimDef in allClaims)
        {
            if (!await _dbContext.RoleClaims.AnyAsync(rc => rc.RoleId == adminRole.Id && rc.ClaimType == claimDef.Type && rc.ClaimValue == claimDef.Value))
            {
                adminClaims.Add(new RoleClaim
                {
                    RoleId = adminRole.Id,
                    ClaimType = claimDef.Type,
                    ClaimValue = claimDef.Value,
                    ClaimDefinitionId = claimDef.Id
                });
            }
        }

        if (adminClaims.Any())
        {
            await _dbContext.RoleClaims.AddRangeAsync(adminClaims);
            await _dbContext.SaveChangesAsync();
            Console.WriteLine($"Assigned {adminClaims.Count} claims to Admin role.");
        }
    }
}

