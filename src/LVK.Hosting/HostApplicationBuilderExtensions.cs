using System.Diagnostics;

using LVK.Hosting.ConsoleApplications;
using LVK.Hosting.ConsoleApplications.Internal;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Hosting;

public static class HostApplicationBuilderExtensions
{
    private static readonly object _key = new();

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

        builder.Bootstrap(new ModuleBootstrapper());

        return registry;
    }

    public static IHostApplicationBuilder AddStandardConfigurationSources<TProgram>(this IHostApplicationBuilder builder)
        where TProgram : class
    {
        IConfigurationSource[] beforeJson = builder.Configuration.Sources.TakeWhile(source => source is not JsonConfigurationSource).ToArray();
        IConfigurationSource[] afterJson = builder.Configuration.Sources.SkipWhile(source => source is not JsonConfigurationSource).SkipWhile(source => source is JsonConfigurationSource).ToArray();
        builder.Configuration.Sources.Clear();
        foreach (IConfigurationSource source in beforeJson)
        {
            builder.Configuration.Sources.Add(source);
        }

        string environmentName = builder.Environment.EnvironmentName;

        builder.Configuration.SetBasePath(Path.GetDirectoryName(typeof(TProgram).Assembly.Location)!);
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{Environment.MachineName}.{environmentName}.json", optional: true, reloadOnChange: true);

        foreach (IConfigurationSource source in afterJson)
        {
            builder.Configuration.Sources.Add(source);
        }

        if (builder.Environment.IsDevelopment() || Debugger.IsAttached)
        {
            builder.Configuration.AddUserSecrets<TProgram>();
        }

        return builder;
    }

    public static IHostApplicationBuilder AddConsoleApplication<T>(this IHostApplicationBuilder builder, Action<T>? configure = null)
        where T : class, IConsoleApplication
    {
        builder.Services.AddHostedService<ConsoleApplicationHostedService>();
        builder.Services.Configure<ConsoleApplicationHostedServiceOptions>(options => options.SetConsoleApplication(configure));
        return builder;
    }

    public static IHostApplicationBuilder AddConsoleCommand<T>(this IHostApplicationBuilder builder, Action<T>? configure = null)
        where T : class, IConsoleApplication
    {
        builder.Services.AddHostedService<ConsoleApplicationHostedService>();
        builder.Services.Configure<ConsoleApplicationHostedServiceOptions>(options => options.AddCommand(configure));
        return builder;
    }

    public static IHostApplicationBuilder AddConsoleCommands<TProgram>(this IHostApplicationBuilder builder)
        where TProgram : class
    {
        builder.Services.AddHostedService<ConsoleApplicationHostedService>();

        foreach (Type type in typeof(TProgram).Assembly.GetTypes())
        {
            if (type.IsAssignableTo(typeof(IConsoleApplication)) && !type.IsAbstract)
            {
                builder.Services.Configure<ConsoleApplicationHostedServiceOptions>(options => options.AddCommand(type));
            }
        }

        return builder;
    }
}