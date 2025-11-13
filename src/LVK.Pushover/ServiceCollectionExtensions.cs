using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LVK.Pushover;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPushoverClient(Action<PushoverNotificationOptions> configure)
        {
            services.AddHttpClient();
            services.Configure(configure);
            services.TryAddSingleton<IPushover, Pushover>();

            return services;
        }

        public IServiceCollection AddPushoverClient(IConfigurationSection configurationSection) => AddPushoverClient(services, configurationSection.Bind);
    }
}