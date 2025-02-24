using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace core.Common.Extensions;

public static class ValidatorExtensions
{
    public static IServiceCollection AddDaiminhValidators(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        try
        {
            services.AddFluentValidationAutoValidation(config => { config.DisableDataAnnotationsValidation = true; });

            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic && !string.IsNullOrEmpty(a.Location))
                .ToArray();

            foreach (var assembly in assemblies)
                try
                {
                    var validatorTypes = assembly.GetTypes()
                        .Where(t => t is
                                    {
                                        IsClass: true, IsAbstract: false, IsGenericType: false,
                                        BaseType.IsGenericType: true
                                    }
                                    && t.BaseType.GetGenericTypeDefinition() == typeof(AbstractValidator<>));

                    foreach (var validatorType in validatorTypes)
                    {
                        var entityType = validatorType.BaseType!.GetGenericArguments()[0];

                        var serviceType = typeof(IValidator<>).MakeGenericType(entityType);
                        services.AddScoped(serviceType, validatorType);
                    }
                }
                catch (Exception)
                {
                    continue;
                }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to register validators", ex);
        }

        return services;
    }

    public static async Task<bool> ValidateAndReturnView<T>(
        this Controller controller,
        IValidator<T> validator,
        T model) where T : class
    {
        ArgumentNullException.ThrowIfNull(validator);
        ArgumentNullException.ThrowIfNull(model);

        var validationResult = await validator.ValidateAsync(model);

        if (validationResult.IsValid) return false;

        validationResult.AddToModelState(controller.ModelState);
        return true;
    }
}