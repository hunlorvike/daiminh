using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using shared.Attributes;

namespace shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDaiminhService(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        try
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var serviceTypes = assemblies
                .SelectMany(a => a.GetTypes())
                .Where(t => t is { IsClass: true, IsAbstract: false, IsInterface: false });

            foreach (var type in serviceTypes)
            {
                var lifetime = GetLifetimeFromBaseType(type);
                if (lifetime == null) continue;

                var interfaces = type.GetInterfaces()
                    .Where(i => i != typeof(IDisposable) && i != typeof(IAsyncDisposable));

                foreach (var @interface in interfaces)
                    services.Add(new ServiceDescriptor(@interface, type, lifetime.Value));

                services.Add(new ServiceDescriptor(type, type, lifetime.Value));
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            throw new InvalidOperationException("Failed to load DI", ex);
        }

        return services;
    }

    private static ServiceLifetime? GetLifetimeFromBaseType(Type type)
    {
        var currentType = type;
        while (currentType != null)
        {
            var attribute = currentType.GetCustomAttribute<DependencyAttribute>();
            if (attribute != null) return attribute.Lifetime;
            currentType = currentType.BaseType;
        }

        return null;
    }
}