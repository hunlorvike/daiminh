using System.Reflection;
using AutoRegister;
using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;
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
            // Policy "AdminAccess" để truy cập bất kỳ Controller nào trong Admin Area
            // Policy này yêu cầu người dùng có Claim "Admin.Access".
            options.AddPolicy("AdminAccess", policy =>
            {
                policy.Requirements.Add(new PermissionRequirement("Admin.Access"));
            });

            // FAQ
            options.AddPolicy("FAQ.View", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.View")));
            options.AddPolicy("FAQ.Create", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.Create")));
            options.AddPolicy("FAQ.Edit", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.Edit")));
            options.AddPolicy("FAQ.Delete", policy => policy.Requirements.Add(new PermissionRequirement("FAQ.Delete")));

            // Article
            options.AddPolicy("Article.View", policy => policy.Requirements.Add(new PermissionRequirement("Article.View")));
            options.AddPolicy("Article.Create", policy => policy.Requirements.Add(new PermissionRequirement("Article.Create")));
            options.AddPolicy("Article.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Article.Edit")));
            options.AddPolicy("Article.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Article.Delete")));

            // Attribute
            options.AddPolicy("Attribute.View", policy => policy.Requirements.Add(new PermissionRequirement("Attribute.View")));
            options.AddPolicy("Attribute.Create", policy => policy.Requirements.Add(new PermissionRequirement("Attribute.Create")));
            options.AddPolicy("Attribute.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Attribute.Edit")));
            options.AddPolicy("Attribute.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Attribute.Delete")));

            // Banner
            options.AddPolicy("Banner.View", policy => policy.Requirements.Add(new PermissionRequirement("Banner.View")));
            options.AddPolicy("Banner.Create", policy => policy.Requirements.Add(new PermissionRequirement("Banner.Create")));
            options.AddPolicy("Banner.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Banner.Edit")));
            options.AddPolicy("Banner.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Banner.Delete")));

            // Newsletter
            options.AddPolicy("Newsletter.View", policy => policy.Requirements.Add(new PermissionRequirement("Newsletter.View")));
            options.AddPolicy("Newsletter.Create", policy => policy.Requirements.Add(new PermissionRequirement("Newsletter.Create")));
            options.AddPolicy("Newsletter.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Newsletter.Edit")));
            options.AddPolicy("Newsletter.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Newsletter.Delete")));

            // Page
            options.AddPolicy("Page.View", policy => policy.Requirements.Add(new PermissionRequirement("Page.View")));
            options.AddPolicy("Page.Create", policy => policy.Requirements.Add(new PermissionRequirement("Page.Create")));
            options.AddPolicy("Page.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Page.Edit")));
            options.AddPolicy("Page.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Page.Delete")));

            // PopupModal
            options.AddPolicy("PopupModal.View", policy => policy.Requirements.Add(new PermissionRequirement("PopupModal.View")));
            options.AddPolicy("PopupModal.Create", policy => policy.Requirements.Add(new PermissionRequirement("PopupModal.Create")));
            options.AddPolicy("PopupModal.Edit", policy => policy.Requirements.Add(new PermissionRequirement("PopupModal.Edit")));
            options.AddPolicy("PopupModal.Delete", policy => policy.Requirements.Add(new PermissionRequirement("PopupModal.Delete")));

            // Product
            options.AddPolicy("Product.View", policy => policy.Requirements.Add(new PermissionRequirement("Product.View")));
            options.AddPolicy("Product.Create", policy => policy.Requirements.Add(new PermissionRequirement("Product.Create")));
            options.AddPolicy("Product.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Product.Edit")));
            options.AddPolicy("Product.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Product.Delete")));

            // ProductReview
            options.AddPolicy("ProductReview.View", policy => policy.Requirements.Add(new PermissionRequirement("ProductReview.View")));
            options.AddPolicy("ProductReview.Edit", policy => policy.Requirements.Add(new PermissionRequirement("ProductReview.Edit")));
            options.AddPolicy("ProductReview.Delete", policy => policy.Requirements.Add(new PermissionRequirement("ProductReview.Delete")));

            // ProductVariation
            options.AddPolicy("ProductVariation.View", policy => policy.Requirements.Add(new PermissionRequirement("ProductVariation.View")));
            options.AddPolicy("ProductVariation.Create", policy => policy.Requirements.Add(new PermissionRequirement("ProductVariation.Create")));
            options.AddPolicy("ProductVariation.Edit", policy => policy.Requirements.Add(new PermissionRequirement("ProductVariation.Edit")));
            options.AddPolicy("ProductVariation.Delete", policy => policy.Requirements.Add(new PermissionRequirement("ProductVariation.Delete")));

            // Setting
            options.AddPolicy("Setting.Manage", policy => policy.Requirements.Add(new PermissionRequirement("Setting.Manage")));

            // Slide
            options.AddPolicy("Slide.View", policy => policy.Requirements.Add(new PermissionRequirement("Slide.View")));
            options.AddPolicy("Slide.Create", policy => policy.Requirements.Add(new PermissionRequirement("Slide.Create")));
            options.AddPolicy("Slide.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Slide.Edit")));
            options.AddPolicy("Slide.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Slide.Delete")));

            // Testimonial
            options.AddPolicy("Testimonial.View", policy => policy.Requirements.Add(new PermissionRequirement("Testimonial.View")));
            options.AddPolicy("Testimonial.Create", policy => policy.Requirements.Add(new PermissionRequirement("Testimonial.Create")));
            options.AddPolicy("Testimonial.Edit", policy => policy.Requirements.Add(new PermissionRequirement("Testimonial.Edit")));
            options.AddPolicy("Testimonial.Delete", policy => policy.Requirements.Add(new PermissionRequirement("Testimonial.Delete")));

            // User
            options.AddPolicy("User.Manage", policy => policy.Requirements.Add(new PermissionRequirement("User.Manage")));

            // Role
            options.AddPolicy("Role.Manage", policy => policy.Requirements.Add(new PermissionRequirement("Role.Manage")));

            // ClaimDefinition
            options.AddPolicy("ClaimDefinition.Manage", policy => policy.Requirements.Add(new PermissionRequirement("ClaimDefinition.Manage")));
        });

        return services;

    }

    public static IServiceCollection AddCustomServices(this IServiceCollection services)
    {
        services.AddAutoregister([Assembly.GetExecutingAssembly(), typeof(ApplicationDbContext).Assembly]);

        return services;
    }
}

