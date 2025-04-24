using domain.Entities;
using FluentValidation;
using FluentValidation.AspNetCore;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Minio;
using Serilog;
using Serilog.Events;
using web.Areas.Admin.Services;
using web.Areas.Admin.Validators.User;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.Console()
    .WriteTo.File("Logs/logs-.log", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true,
        restrictedToMinimumLevel: LogEventLevel.Warning,
        fileSizeLimitBytes: 10 * 1024 * 1024,
        outputTemplate:
        "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string not found."));
});


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:DefaultConnection"];
    options.InstanceName = "DaiMinhCache_";
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidationAutoValidation(config =>
{
    config.DisableDataAnnotationsValidation = true;
}).AddFluentValidationClientsideAdapters();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UserCreateViewModelValidator>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IMinioService, MinioService>();
builder.Services.AddScoped<IMediaService, MediaService>();

builder.Services.AddAuthentication()
    .AddCookie("DaiMinhCookies", options =>
    {
        options.Cookie.Name = "DaiMinhCookies";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.LoginPath = "/Admin/Account/Login";
        options.LogoutPath = "/Admin/Account/Logout";
        options.AccessDeniedPath = "/Error/Forbidden";
    });

var minioEndpoint = builder.Configuration["Minio:Endpoint"];
var minioAccessKey = builder.Configuration["Minio:AccessKey"];
var minioSecretKey = builder.Configuration["Minio:SecretKey"];

builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioEndpoint)
    .WithCredentials(minioAccessKey, minioSecretKey)
    .WithSSL(false)
    .WithHttpClient(new HttpClient())
);

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Exception handler for non-development environments
    app.UseHsts(); // HSTS settings for production
}

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles();

app.UseRouting(); // Enable routing

// Authentication must come before Authorization
app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization

// Define routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

Log.Information("Starting web host");
await app.RunAsync();