using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

public interface IModuleBootstrapper
{
    void Bootstrap(IHostApplicationBuilder builder);
}