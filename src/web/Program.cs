using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;
using web.Areas.Admin.Validators;
using web.Configs;
using web.Extensions;

SerilogConfig.Configure();

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

builder.Host.UseSerilog();

var cacheProvider = builder.Configuration.GetValue<string>("CacheProvider:Type")?.ToLowerInvariant();

switch (cacheProvider)
{
    case "redis":
        var redisConnectionString = builder.Configuration.GetConnectionString("RedisConnection");
        if (string.IsNullOrEmpty(redisConnectionString))
        {
            redisConnectionString = builder.Configuration["Redis:DefaultConnection"];
        }
        if (string.IsNullOrEmpty(redisConnectionString))
        {
            throw new InvalidOperationException("Redis connection string 'Redis:DefaultConnection' or 'ConnectionStrings:RedisConnection' not found.");
        }
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = builder.Configuration["Redis:InstanceName"];
        });
        Log.Information("Đã cấu hình Redis Cache.");
        break;

    case "sqlserver":
    case "memory":
    default:
        break;
}

builder.Services
    .AddDatabase(builder.Configuration)
    .AddIdentity(builder.Configuration)
    .AddMinioService(builder.Configuration)
    .AddAuthenticationServices()
    .AddCustomServices();

builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidation(config =>
{
    config.AutomaticValidationEnabled = false;
});
builder.Services.AddValidatorsFromAssemblyContaining<UserViewModelValidator>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCustomMiddlewares();
app.MapDefaultRoutes();

// Seed Data on startup
using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDataSeeder>();
    await seeder.SeedAllAsync();
}

await app.RunAsync();
