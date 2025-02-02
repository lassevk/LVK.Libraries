using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Jobs.PostgreSQL;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddPostgresJobStorage(this IHostApplicationBuilder builder, string connectionString)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));
        _ = connectionString ?? throw new ArgumentNullException(nameof(connectionString));

        builder.Services.AddSingleton<IJobStorage, PostgresJobStorage>();
        builder.Services.AddDbContextFactory<PostgresDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });

        return builder;
    }
}