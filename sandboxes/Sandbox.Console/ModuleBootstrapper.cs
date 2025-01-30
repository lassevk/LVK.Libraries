using LasseVK.Bootstrapping;
using LasseVK.Jobs;

using Microsoft.Extensions.Hosting;

using Sandbox.Console.Jobs;

namespace Sandbox.Console;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new LasseVK.Jobs.ModuleBootstrapper());

        builder.Services.AddMemoryJobStorage();

        builder.Services.AddJobHandler<CalculateOperand1Job, CalculateOperand1Handler>();
        builder.Services.AddJobHandler<CalculateOperand2Job, CalculateOperand2Handler>();
        builder.Services.AddJobHandler<CalculateSumJob, CalculateSumJobHandler>();
    }
}