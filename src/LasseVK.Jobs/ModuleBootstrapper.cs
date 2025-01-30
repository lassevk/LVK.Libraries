using LasseVK.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Jobs;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IJobManager, JobManager>();
    }
}