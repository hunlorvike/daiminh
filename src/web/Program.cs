using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
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

// Cấu hình Options Pattern
builder.Services.AddApplicationOptions(builder.Configuration);

// Cấu hình Redis Cache từ Options
var redisOptions = builder.Configuration.GetSection("Redis").Get<RedisSettings>();
if (redisOptions != null && !string.IsNullOrEmpty(redisOptions.ConnectionString))
{
    builder.Services.AddStackExchangeRedisCache(options =>
    {
        options.Configuration = redisOptions.ConnectionString;
        options.InstanceName = redisOptions.InstanceName;
    });
    Log.Information("Đã cấu hình Redis Cache với ConnectionString: {ConnectionString}",
        redisOptions.ConnectionString);
}

builder.Services
    .AddDatabase(builder.Configuration)
    .AddIdentity(builder.Configuration)
    .AddMinioService(builder.Configuration)
    .AddAuthenticationServices()
    .AddAuthorizationServices()
    .AddCustomServices();

builder.Services.AddControllersWithViews();

builder.Services.AddValidatorsFromAssemblyContaining<UserViewModelValidator>();

builder.Services.Configure<FluentValidationMvcConfiguration>(config =>
{
    config.DisableDataAnnotationsValidation = true;
    config.AutomaticValidationEnabled = false;
});

ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;
ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
builder.Services.AddScoped<IUrlHelper>(x =>
{
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext!);
});

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCustomMiddlewares();
app.MapDefaultRoutes();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationDataSeeder>();
    await seeder.SeedAllAsync();
}

await app.RunAsync();