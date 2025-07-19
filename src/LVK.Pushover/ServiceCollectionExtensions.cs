using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LVK.Pushover;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPushoverClient(this IServiceCollection services, Action<PushoverNotificationOptions> configure)
    {
        services.AddHttpClient();
        services.Configure(configure);
        services.TryAddSingleton<IPushover, Pushover>();

        return services;
    }

    public static IServiceCollection AddPushoverClient(this IServiceCollection services, IConfigurationSection configurationSection)
        => AddPushoverClient(services, configurationSection.Bind);
}