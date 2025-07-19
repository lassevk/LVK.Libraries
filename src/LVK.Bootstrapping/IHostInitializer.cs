using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

public interface IHostInitializer<in T>
    where T : IHost
{
    Task InitializeAsync(T host, CancellationToken cancellationToken);
}