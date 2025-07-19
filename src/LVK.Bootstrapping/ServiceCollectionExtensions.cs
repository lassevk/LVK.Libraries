using LVK.Bootstrapping.ConsoleApplications;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleApplication<T>(this IServiceCollection services)
        where T : class, IConsoleApplication
    {
        services.AddSingleton<IConsoleApplication, T>();
        services.TryAddSingleton<IHostedService, ConsoleApplicationHostedService>();

        return services;
    }
}