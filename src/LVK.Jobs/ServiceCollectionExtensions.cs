using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.Jobs;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddJobHandler<TJob, TJobHandler>()
            where TJob : Job
            where TJobHandler : class, IJobHandler<TJob>
        {
            _ = services ?? throw new ArgumentNullException(nameof(services));

            services.AddScoped<IJobHandler<TJob>, TJobHandler>();

            return services;
        }

        public IServiceCollection AddJobHandlers<T>()
        {
            Assembly assembly = typeof(T).Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAbstract || !type.IsAssignableTo(typeof(IJobHandler)))
                {
                    continue;
                }

                var interfaces = type.GetInterfaces().Where(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IJobHandler<>)).ToList();
                foreach (Type handlerType in interfaces)
                {
                    services.AddScoped(handlerType, type);
                }
            }

            return services;
        }
    }
}