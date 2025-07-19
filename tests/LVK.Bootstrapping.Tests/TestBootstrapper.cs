using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping.Tests;

public class TestBootstrapper : IModuleBootstrapper
{
    public int BootstrapCount = 0;
    
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        BootstrapCount++;
    }
}