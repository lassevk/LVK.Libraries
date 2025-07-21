using Microsoft.Extensions.Hosting;

namespace LVK.Hosting;

public interface IModuleInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}