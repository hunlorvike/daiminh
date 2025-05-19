using domain.Entities;
using infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Minio;
using web.Areas.Admin.Services;
using web.Areas.Admin.Services.Interfaces;

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
        services.AddSingleton<ICacheService, DistributedCacheService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IMinioService, MinioService>();
        services.AddScoped<IMediaService, MediaService>();
        services.AddScoped<ISettingService, SettingService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IAttributeService, AttributeService>();
        services.AddScoped<ITagService, TagService>();
        services.AddScoped<IFAQService, FAQService>();
        services.AddScoped<IBannerService, BannerService>();
        services.AddScoped<IContactService, ContactService>();
        services.AddScoped<ITestimonialService, TestimonialService>();
        services.AddScoped<INewsletterService, NewsletterService>();
        services.AddScoped<ISlideService, SlideService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IProductReviewService, ProductReviewService>();
        services.AddScoped<IArticleService, ArticleService>();

        return services;
    }
}

