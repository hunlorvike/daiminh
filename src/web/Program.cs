using System.Reflection;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;

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

#region Services Configuration

#region Database Configuration

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found."));
});

#endregion

#region Redis Configuration

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:DefaultConnection"]; ;
    options.InstanceName = "DaiMinhCache_";
});

#endregion

#region MVC and Validation

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllersWithViews();
builder.Services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly()]);

builder.Services.AddHttpContextAccessor();

#endregion
#endregion

#region Authentication and Authorization

builder.Services.AddAuthentication()
    .AddCookie("DaiMinhCookies", options =>
    {
        options.Cookie.Name = "DaiMinhCookies";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.LoginPath = "/Admin/Auth/Login";
        options.LogoutPath = "/Admin/Auth/Logout";
        options.AccessDeniedPath = "/Admin/Auth/AccessDenied";
    });

#endregion

#region Dependency Injection (Services Registration)

#endregion

var app = builder.Build();

app.UseSerilogRequestLogging();

#region Middleware Configuration

#region Error Handling and Security

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Exception handler for non-development environments
    app.UseHsts(); // HSTS settings for production
}

#endregion

#region HTTPS and Static Files

app.UseHttpsRedirection(); // Redirect HTTP to HTTPS
app.UseStaticFiles();

#endregion

#region Routing, Authentication, and Authorization

app.UseRouting(); // Enable routing

app.UseAuthentication(); // Enable authentication
app.UseAuthorization(); // Enable authorization


#endregion

#region Endpoint Mapping

// Define routes
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

#endregion

#endregion

app.Run();