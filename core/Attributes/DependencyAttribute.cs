namespace core.Attributes;

using Microsoft.Extensions.DependencyInjection;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class DependencyAttribute(ServiceLifetime lifetime) : Attribute
{
    public ServiceLifetime Lifetime { get; } = lifetime;
}

[Dependency(ServiceLifetime.Singleton)]
public abstract class SingletonService
{
}

[Dependency(ServiceLifetime.Scoped)]
public abstract class ScopedService
{
}

[Dependency(ServiceLifetime.Transient)]
public abstract class TransientService
{
}