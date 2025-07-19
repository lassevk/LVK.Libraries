using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LVK.Jobs.PostgreSQL;

public static class JobManagerConfigurationExtensions
{
    public static JobManagerConfiguration UsePostgreSql(this JobManagerConfiguration configuration, string connectionString)
    {
        configuration.JobStorageFactory = CreatePostgresJobStorage;

        configuration.Builder.Services.AddDbContextFactory<PostgresDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return configuration;
    }

    private static IJobStorage CreatePostgresJobStorage(IServiceProvider serviceProvider) => ActivatorUtilities.CreateInstance<PostgresJobStorage>(serviceProvider);
}