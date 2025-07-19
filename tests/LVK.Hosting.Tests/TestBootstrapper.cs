using LVK.Hosting;

using Microsoft.Extensions.Hosting;

namespace LVK.Hosting.Tests;

public class TestBootstrapper : IModuleBootstrapper
{
    public int BootstrapCount = 0;

    public void Bootstrap(IHostApplicationBuilder builder)
    {
        BootstrapCount++;
    }
}