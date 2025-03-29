using System.Reflection;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using web;
using web.Areas.Admin.Services;
using web.Middlewares;

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
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found."));
});


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:DefaultConnection"]; ;
    options.InstanceName = "DaiMinhCache_";
});


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IRedirectService, RedirectService>();
builder.Services.AddScoped<ISeoService, SeoService>();
builder.Services.AddHostedService<SeoAnalyticsBackgroundService>();
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

app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization

app.UseExceptionHandling();
app.UseRedirectMiddleware();

// Define routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();