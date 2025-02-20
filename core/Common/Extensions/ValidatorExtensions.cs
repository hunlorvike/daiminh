using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace core.Common.Extensions;

public static class ValidatorExtensions
{
    public static IServiceCollection AddValidators(this IServiceCollection services, params Assembly[]? assemblies)
    {
        ArgumentNullException.ThrowIfNull(services);

        try
        {
            if (assemblies == null || assemblies.Length == 0) assemblies = [Assembly.GetExecutingAssembly()];

            services.AddFluentValidationAutoValidation();

            IEnumerable<Type> validatorTypes = assemblies
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract &&
                               !type.IsInterface &&
                               type.BaseType != null &&
                               type.BaseType.IsGenericType &&
                               type.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>));

            foreach (var validatorType in validatorTypes)
            {
                var modelType = validatorType.BaseType?.GetGenericArguments().FirstOrDefault();
                if (modelType == null) continue;

                var interfaceType = typeof(IValidator<>).MakeGenericType(modelType);
                services.AddScoped(interfaceType, validatorType);
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            throw new InvalidOperationException("Failed to load validator types", ex);
        }

        return services;
    }
}