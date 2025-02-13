using LasseVK.Bootstrapping.ConsoleApplications;

using Microsoft.Extensions.DependencyInjection;

namespace LasseVK.Bootstrapping;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddConsoleApplication<T>(this IServiceCollection services)
        where T : class, IConsoleApplication
    {
        services.AddSingleton<IConsoleApplication, T>();
        return services;
    }
}