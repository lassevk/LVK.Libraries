using JetBrains.Annotations;

using LasseVK.Bootstrapping.CommandLineArguments;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

[PublicAPI]
public static class HostApplicationBuilderExtensions
{
    private static readonly object _key = new();

    public static IHostApplicationBuilder UseCommandLineArguments<[MeansImplicitUse] T>(this IHostApplicationBuilder builder)
        where T : class, new()
    {
        builder.Services.AddSingleton(CommandLineArgumentsHelper.CreateArguments<T>);
        return builder;
    }

    public static IHostApplicationBuilder Bootstrap(this IHostApplicationBuilder builder, IModuleBootstrapper bootstrapper)
    {
        HashSet<Type> registry = GetRegistry(builder);
        if (!registry.Add(bootstrapper.GetType()))
        {
            return builder;
        }

        bootstrapper.Bootstrap(builder);
        return builder;
    }

    private static HashSet<Type> GetRegistry(IHostApplicationBuilder builder)
    {
        if (builder.Services.FirstOrDefault(sd => sd.IsKeyedService && sd.ServiceKey == _key)?.KeyedImplementationInstance is HashSet<Type> registry)
        {
            return registry;
        }

        registry = new();
        builder.Services.AddKeyedSingleton(_key, registry);

        return registry;
    }
}