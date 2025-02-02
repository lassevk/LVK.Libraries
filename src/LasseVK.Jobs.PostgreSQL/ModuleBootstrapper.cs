using LasseVK.Bootstrapping;

using Microsoft.Extensions.Hosting;

namespace LasseVK.Jobs.PostgreSQL;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Bootstrap(new LasseVK.Jobs.ModuleBootstrapper());
    }
}