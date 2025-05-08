using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Serilog;
using web.Areas.Admin.Validators.User;
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

builder.Services.AddDatabase(builder.Configuration)
    .AddMemoryCache()
    .AddDistributedMemoryCache()
    .AddRedis(builder.Configuration)
    .AddMinioService(builder.Configuration)
    .AddAuthenticationServices()
    .AddCustomServices();

builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
});
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateViewModelValidator>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseCustomMiddlewares();
app.MapDefaultRoutes();

await app.RunAsync();
