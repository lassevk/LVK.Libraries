using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LVK.Blazor;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddLocalizationService()
        {
            services.AddTransient<ILocalizationServiceProvider, LocalizationServiceProvider>();
            services.AddHttpContextAccessor();

            return services;
        }

        public IServiceCollection AddGlobalLocalizations<T>()
            where T : class
        {
            services.TryAddSingleton<GlobalResourceProvider>();
            services.Configure<GlobalResourceProviderConfiguration>(c => c.Add(typeof(T)));
            return services;
        }
    }
}