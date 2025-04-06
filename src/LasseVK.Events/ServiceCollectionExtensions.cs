using Microsoft.Extensions.DependencyInjection;

namespace LasseVK.Events;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddSingleton<IEvents, Events>();

        return services;
    }
}