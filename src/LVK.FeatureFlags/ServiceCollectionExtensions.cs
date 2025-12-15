using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace LVK.FeatureFlags;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddFeatureFlags(Action<FeatureFlagsOptions>? configure = null)
        {
            OptionsBuilder<FeatureFlagsOptions> optionsRegistration = services.AddOptions<FeatureFlagsOptions>();
            if (configure != null)
            {
                optionsRegistration.Configure(configure);
            }

            services.TryAddSingleton<IFeatureFlags, FeatureFlags>();

            return services;
        }
    }
}