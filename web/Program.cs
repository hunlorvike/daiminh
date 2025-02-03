using core.Interfaces;
using core.Services;
using infrastructure.Data;
using infrastructure.Data.Repositories;
using infrastructure.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? throw new InvalidOperationException("Connection string not found.")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); // register unit of work

builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>)); // register generic repository

builder.Services.AddScoped<ContactService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // HSTS settings for production
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
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

app.Run();

// (routing → static files → authorization → endpoint mapping)