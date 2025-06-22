using shared.Models;

namespace web.Extensions;

public static class OptionsServiceExtensions
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<GeneralSettings>(configuration.GetSection("GeneralSettings"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<MinioSettings>(configuration.GetSection("Minio"));
        services.Configure<RedisSettings>(configuration.GetSection("Redis"));
        services.Configure<ApiKeySettings>(configuration.GetSection("ApiKeys"));
        services.Configure<DefaultSeoSettings>(configuration.GetSection("DefaultSeo"));
        services.Configure<PaginationSettings>(configuration.GetSection("Pagination"));
        services.Configure<SocialMediaSettings>(configuration.GetSection("SocialMedia"));

        services.AddOptionsWithValidation<GeneralSettings>("GeneralSettings", configuration);
        services.AddOptionsWithValidation<EmailSettings>("EmailSettings", configuration);
        services.AddOptionsWithValidation<MinioSettings>("Minio", configuration);
        services.AddOptionsWithValidation<RedisSettings>("Redis", configuration);
        services.AddOptionsWithValidation<PaginationSettings>("Pagination", configuration);

        return services;
    }

    private static IServiceCollection AddOptionsWithValidation<T>(
        this IServiceCollection services,
        string sectionName,
        IConfiguration configuration) where T : class, new()
    {
        services.Configure<T>(configuration.GetSection(sectionName));

        services.PostConfigure<T>(options =>
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(options);
            var validationResults = new List<System.ComponentModel.DataAnnotations.ValidationResult>();

            if (!System.ComponentModel.DataAnnotations.Validator.TryValidateObject(
                options, validationContext, validationResults, true))
            {
                var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                throw new InvalidOperationException($"Configuration validation failed for {sectionName}: {errors}");
            }
        });

        return services;
    }
}