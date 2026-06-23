using System.Collections.Concurrent;

using JetBrains.Annotations;

using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

[PublicAPI]
public static class HostApplicationBuilderExtensions
{
    private static readonly Guid _registryId = Guid.NewGuid();

    extension<T>(T builder)
        where T : IHostApplicationBuilder
    {
        public T Bootstrap(IApplicationBootstrapper<T> bootstrapper)
        {
            HashSet<Type> registry = GetRegistry(builder.Properties);
            if (!registry.Add(bootstrapper.GetType()))
            {
                return builder;
            }

            bootstrapper.Bootstrap(builder);
            return builder;
        }
    }

    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder Bootstrap(IModuleBootstrapper bootstrapper)
        {
            HashSet<Type> registry = GetRegistry(builder.Properties);
            if (!registry.Add(bootstrapper.GetType()))
            {
                return builder;
            }

            bootstrapper.Bootstrap(builder);
            return builder;
        }
    }

    private static HashSet<Type> GetRegistry(IDictionary<object, object> properties)
    {
        if (properties.TryGetValue(_registryId, out object? registryObject))
        {
            return (HashSet<Type>)registryObject;
        }

        var registry = new HashSet<Type>();
        properties[_registryId] = registry;
        return registry;
    }
}