using LasseVK.Bootstrapping;
using LasseVK.Jobs;
using LasseVK.Jobs.PostgreSQL;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Sandbox.Console;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Configuration.AddUserSecrets<Program>();

        builder.Bootstrap(new LasseVK.Jobs.ModuleBootstrapper());
        builder.Bootstrap(new LasseVK.Jobs.PostgreSQL.ModuleBootstrapper());

        builder.Services.AddJobHandlers<Program>();

        string connectionString = builder.Configuration.GetConnectionString("Jobs") ?? throw new InvalidOperationException("No jobs connection string");
        builder.AddJobManager(configuration =>
        {
            configuration.UsePostgreSql(connectionString);
        });
    }
}