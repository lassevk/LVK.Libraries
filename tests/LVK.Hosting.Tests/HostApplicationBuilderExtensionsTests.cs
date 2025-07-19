using LVK.Hosting;

using Microsoft.Extensions.Hosting;

namespace LVK.Hosting.Tests;

public class Tests
{
    [Test]
    public void Bootstrap_CalledOnce_CallsBootstrapper()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        var bootstrapper = new TestBootstrapper();

        builder.Bootstrap(bootstrapper);

        Assert.That(bootstrapper.BootstrapCount, Is.EqualTo(1));
    }

    [Test]
    public void Bootstrap_CalledTwice_OnlyCallsBootstrapperOnce()
    {
        HostApplicationBuilder builder = Host.CreateApplicationBuilder();
        var bootstrapper = new TestBootstrapper();

        builder.Bootstrap(bootstrapper);
        builder.Bootstrap(bootstrapper);

        Assert.That(bootstrapper.BootstrapCount, Is.EqualTo(1));
    }
}