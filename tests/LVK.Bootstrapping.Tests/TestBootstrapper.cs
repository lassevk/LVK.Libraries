using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping.Tests;

public class TestBootstrapper : IModuleBootstrapper
{
    public int BootstrapCount = 0;

    public Task BootstrapAsync(IHostApplicationBuilder builder)
    {
        BootstrapCount++;
        return Task.CompletedTask;
    }
}