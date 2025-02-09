using core.Interceptors;
using core.Interfaces;
using core.Services;
using infrastructure.Data;
using infrastructure.Data.Repositories;
using infrastructure.Data.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Services Configuration

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "AdminAuth";
        options.DefaultChallengeScheme = "AdminAuth";
    })
    .AddCookie("AdminAuth", options =>
    {
        options.LoginPath = "/Admin/Auth/Login";
        options.LogoutPath = "/Admin/Auth/Logout";
        options.AccessDeniedPath = "/Admin/Auth/AccessDenied";
        options.Cookie.Name = "AdminAuthCookie";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    })
    .AddCookie("CustomerAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/AccessDenied";
        options.Cookie.Name = "CustomerAuthCookie";
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromHours(24);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminAuth", policy =>
        policy.RequireAuthenticatedUser()
            .AddAuthenticationSchemes("AdminAuth"));
});

builder.Services.AddSingleton<AuditSaveChangesInterceptor>();

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