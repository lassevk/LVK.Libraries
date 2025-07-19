using Microsoft.Extensions.Hosting;

namespace LVK.Hosting;

public interface IModuleBootstrapper
{
    void Bootstrap(IHostApplicationBuilder builder);
}