using LasseVK.Bootstrapping;
using LasseVK.Jobs;
using LasseVK.Jobs.PostgreSQL;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using Sandbox.Console.Jobs;

namespace Sandbox.Console;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Configuration.AddUserSecrets<Program>();

        builder.Bootstrap(new LasseVK.Jobs.ModuleBootstrapper());
        builder.Bootstrap(new LasseVK.Jobs.PostgreSQL.ModuleBootstrapper());

        builder.Services.AddJobHandler<CalculateOperand1Job, CalculateOperand1Handler>();
        builder.Services.AddJobHandler<CalculateOperand2Job, CalculateOperand2Handler>();
        builder.Services.AddJobHandler<CalculateSumJob, CalculateSumJobHandler>();

        string connectionString = builder.Configuration.GetConnectionString("Jobs") ?? throw new InvalidOperationException("No jobs connection string");
        builder.AddPostgresJobStorage(connectionString);
    }
}