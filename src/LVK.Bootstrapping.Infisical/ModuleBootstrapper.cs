using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping.Infisical;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public async Task BootstrapAsync(IHostApplicationBuilder builder)
    {
        await builder.AddInfisical();
    }
}