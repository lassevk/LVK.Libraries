using LasseVK.Bootstrapping.ConsoleApplications;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleApplication<T>(this IServiceCollection services)
        where T : class, IConsoleApplication
    {
        services.AddSingleton<IConsoleApplication, T>();
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, ConsoleApplicationHostedService>());

        return services;
    }
}