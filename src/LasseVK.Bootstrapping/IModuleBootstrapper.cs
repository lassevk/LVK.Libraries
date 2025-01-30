using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

public interface IModuleBootstrapper
{
    void Bootstrap(IHostApplicationBuilder builder);
}