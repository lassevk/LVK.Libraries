using System.Collections.Concurrent;

using JetBrains.Annotations;

using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

[PublicAPI]
public static class HostApplicationBuilderExtensions
{
    private static readonly ConcurrentDictionary<object, HashSet<Type>> _registries = [];

    extension(IHostApplicationBuilder builder)
    {
        public async Task<IHostApplicationBuilder> BootstrapAsync(IModuleBootstrapper bootstrapper)
        {
            HashSet<Type> registry = _registries.GetOrAdd(builder, _ => []);
            lock (registry)
            {
                if (!registry.Add(bootstrapper.GetType()))
                {
                    return builder;
                }
            }

            await bootstrapper.BootstrapAsync(builder);
            return builder;
        }
    }
}