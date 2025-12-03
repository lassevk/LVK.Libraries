using System.Collections.Concurrent;

using JetBrains.Annotations;

using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

[PublicAPI]
public static class HostApplicationBuilderExtensions
{
    private static readonly ConcurrentDictionary<object, HashSet<Type>> _registries = [];

    public static IHostApplicationBuilder Bootstrap(this IHostApplicationBuilder builder, IModuleBootstrapper bootstrapper)
    {
        HashSet<Type> registry = _registries.GetOrAdd(builder, _ => []);
        if (!registry.Add(bootstrapper.GetType()))
        {
            return builder;
        }

        bootstrapper.Bootstrap(builder);
        return builder;
    }
}