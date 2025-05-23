using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;
using System.Reflection;
using web.Configs;

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
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
            options.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

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
        services
            .AddAuthentication()
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

    public static IServiceCollection AddAuthorizationServices(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanViewFAQ", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.View")));
            options.AddPolicy("CanCreateFAQ", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.Create")));
            options.AddPolicy("CanEditFAQ", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.Edit")));
            options.AddPolicy("CanDeleteFAQ", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.Delete")));

            options.AddPolicy("AdminAccess", policy =>
            {
                policy.Requirements.Add(new PermissionRequirement("Admin.Access"));
            });
        });
        return services;
    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddAutoregister([Assembly.GetExecutingAssembly(), typeof(ApplicationDbContext).Assembly]);

        return services;
    }
}

