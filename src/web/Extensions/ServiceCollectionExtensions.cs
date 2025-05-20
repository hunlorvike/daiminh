using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;
using System.Reflection;

namespace web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string not found."));
        });

        return services;
    }

    public static IServiceCollection AddIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<User, Role>(options =>
        {
            // Cấu hình Lockout
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // Cấu hình User
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

            // Cấu hình SignIn
            options.SignIn.RequireConfirmedAccount = false;
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders(); // Thêm DefaultTokenProviders để sử dụng các token mặc định như Email Confirmation, Password Reset, etc.

        return services;
    }

    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:DefaultConnection"];
            options.InstanceName = configuration["Redis:InstanceName"];
        });

        return services;
    }

    public static IServiceCollection AddMinioService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(configuration["Minio:Endpoint"])
            .WithCredentials(configuration["Minio:AccessKey"], configuration["Minio:SecretKey"])
            .WithSSL(false)
            .WithHttpClient(new HttpClient())
        );
        return services;
    }

    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddAuthentication()
            .AddCookie("AdminScheme", options =>
            {
                options.Cookie.Name = "AdminCookie";
                options.LoginPath = "/Admin/Account/Login";
                options.AccessDeniedPath = "/Admin/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
            })
            .AddCookie("ClientScheme", options =>
            {
                options.Cookie.Name = "ClientCookie";
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
            });

        return services;
    }


    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddAutoregister(Assembly.GetExecutingAssembly());

        return services;
    }
}

