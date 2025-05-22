using AutoRegister;
using infrastructure.Seeders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace infrastructure;

[Register(ServiceLifetime.Transient)]
public class ApplicationDataSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public ApplicationDataSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SeedAllAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();

            var seeders = services.GetServices<IDataSeeder>()
                                  .OrderBy(s => s.Order)
                                  .ToList();

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync();
            }
        }
    }
}
