using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

internal class HostDecorator : IHost
{
    private readonly IHost _innerHost;

    public HostDecorator(IHost innerHost)
    {
        _innerHost = innerHost ?? throw new ArgumentNullException(nameof(innerHost));
    }

    public void Dispose()
    {
        _innerHost.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken = new())
    {
        IEnumerable<IModuleInitializer> initializers = _innerHost.Services.GetServices<IModuleInitializer>();
        foreach (IModuleInitializer initializer in initializers)
        {
            await initializer.InitializeAsync(cancellationToken);
        }
        await _innerHost.StartAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken = new())
    {
        await _innerHost.StopAsync(cancellationToken);
    }

    public IServiceProvider Services => _innerHost.Services;
}