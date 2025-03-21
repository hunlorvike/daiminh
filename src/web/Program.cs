using System.Reflection;
using application.Interfaces;
using FluentValidation;
using infrastructure;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using shared.Constants;

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

builder.Services.AddScoped<AuditSaveChangesInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found."))
        .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
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
builder.Services.AddValidatorsFromAssemblies([Assembly.GetExecutingAssembly(), Assembly.Load("application")]);

builder.Services.AddHttpContextAccessor();

#endregion
#endregion

#region Authentication and Authorization

builder.Services.AddAuthentication()
    .AddCookie(CookiesConstants.AdminCookieSchema, options =>
    {
        options.Cookie.Name = CookiesConstants.AdminCookieSchema;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.LoginPath = "/Admin/Auth/Login";
        options.LogoutPath = "/Admin/Auth/Logout";
        options.AccessDeniedPath = "/Admin/Auth/AccessDenied";
    })
    .AddCookie(CookiesConstants.UserCookieSchema, options =>
    {
        options.Cookie.Name = CookiesConstants.UserCookieSchema;
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
    });

#endregion

#region Dependency Injection (Services Registration)

builder.Services.Scan(scan => scan
    .FromAssemblyOf<IAuthService>() // Scan from the assembly containing IAuthService
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("Service"))) // Filter classes ending with "Service"
    .AsImplementedInterfaces() // Register as their implemented interfaces
    .WithScopedLifetime()); // Use scoped lifetime

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
    "default",
    "{controller=Home}/{action=Index}/{id?}",
    new { area = "Client" } // Default route for the Client area
);

app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}" // Route for other areas
);

#endregion

#endregion

app.Run();

// (routing → static files → authentication → authorization → endpoint mapping)