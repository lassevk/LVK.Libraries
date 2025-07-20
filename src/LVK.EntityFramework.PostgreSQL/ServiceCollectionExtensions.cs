using Microsoft.Extensions.DependencyInjection;

namespace LVK.EntityFramework.PostgreSQL;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPostgreSqlNotifications(this IServiceCollection services, string connectionString)
    {
        services.AddSingleton<IPostgreSqlNotifications, PostgreSqlNotifications>();
        services.Configure<PostgreSqlNotificationsOptions>(c => c.ConnectionString = connectionString);
        return services;
    }
}