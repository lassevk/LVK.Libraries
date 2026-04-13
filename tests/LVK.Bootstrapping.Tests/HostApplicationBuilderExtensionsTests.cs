using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping.Tests;

public class Tests
{
    [Test]
    public async Task Bootstrap_CalledOnce_CallsBootstrapper()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        var bootstrapper = new TestBootstrapper();

        await builder.BootstrapAsync(bootstrapper);

        Assert.That(bootstrapper.BootstrapCount, Is.EqualTo(1));
    }

    [Test]
    public async Task Bootstrap_CalledTwice_OnlyCallsBootstrapperOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        var bootstrapper = new TestBootstrapper();

        await builder.BootstrapAsync(bootstrapper);
        await builder.BootstrapAsync(bootstrapper);

        Assert.That(bootstrapper.BootstrapCount, Is.EqualTo(1));
    }
}