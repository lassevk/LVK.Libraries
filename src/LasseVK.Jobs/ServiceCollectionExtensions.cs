using Microsoft.Extensions.DependencyInjection;

namespace LasseVK.Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobHandler<TJob, TJobHandler>(this IServiceCollection services)
        where TJob : Job
        where TJobHandler : class, IJobHandler<TJob>
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));

        services.AddScoped<IJobHandler<TJob>, TJobHandler>();

        return services;
    }

    public static IServiceCollection AddMemoryJobStorage(this IServiceCollection services)
    {
        _ = services ?? throw new ArgumentNullException(nameof(services));

        services.AddSingleton<IJobStorage, MemoryJobStorage>();

        return services;
    }
}