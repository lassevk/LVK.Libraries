using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LasseVK.Blazor;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalizationService(this IServiceCollection services)
    {
        services.AddTransient<ILocalizationServiceProvider, LocalizationServiceProvider>();
        return services;
    }
    
    public static IServiceCollection AddGlobalLocalizations<T>(this IServiceCollection services) where T : class
    {
        services.TryAddSingleton<GlobalResourceProvider>();
        services.Configure<GlobalResourceProviderConfiguration>(c => c.Add(typeof(T)));
        return services;
    }
}