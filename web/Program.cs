using core.Common.Constants;
using core.Interceptors;
using core.Interfaces;
using core.Services;
using infrastructure.Data;
using infrastructure.Data.Repositories;
using infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Services Configuration

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

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

builder.Services.AddScoped<AuditSaveChangesInterceptor>();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string not found."))
        .AddInterceptors(serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>());
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // register unit of work

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>)); // register generic repository

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ContactService>();

#endregion

var app = builder.Build();

#region Middleware Configuration

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS settings for production
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Define routes
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}",
    new { area = "Client" }
);

app.MapControllerRoute(
    "areas",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

#endregion

app.Run();

// (routing → static files → authorization → endpoint mapping)