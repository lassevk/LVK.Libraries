using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

public interface IModuleBootstrapper
{
    Task BootstrapAsync(IHostApplicationBuilder builder);
}