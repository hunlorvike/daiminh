namespace web.Extensions;

public static class OptionsServiceExtensions
{
    public static IServiceCollection AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
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