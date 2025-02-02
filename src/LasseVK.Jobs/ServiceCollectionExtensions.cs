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
}