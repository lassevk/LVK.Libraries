using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LVK.Jobs.PostgreSQL;

public static class JobManagerConfigurationExtensions
{
    extension(JobManagerConfiguration configuration)
    {
        public JobManagerConfiguration UsePostgreSql(string connectionString)
        {
            configuration.JobStorageFactory = CreatePostgresJobStorage;

            configuration.Builder.Services.AddDbContextFactory<PostgresDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            return configuration;
        }
    }

    private static IJobStorage CreatePostgresJobStorage(IServiceProvider serviceProvider) => ActivatorUtilities.CreateInstance<PostgresJobStorage>(serviceProvider);
}