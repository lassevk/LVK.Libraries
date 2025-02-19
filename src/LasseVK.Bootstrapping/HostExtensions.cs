using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

public static class HostExtensions
{
    public static async Task InitializeAsync<T>(this T host, CancellationToken cancellationToken = default)
        where T : IHost
    {
        IEnumerable<IHostInitializer<T>> initializers = host.Services.GetServices<IHostInitializer<T>>();
        foreach (IHostInitializer<T> initializer in initializers)
        {
            await initializer.InitializeAsync(host, cancellationToken);
        }
    }
}