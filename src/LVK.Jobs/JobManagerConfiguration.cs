using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Jobs;

public class JobManagerConfiguration
{
    public const string ConfigurationKey = "Jobs:Manager";
    public required IHostApplicationBuilder Builder { get; init; }

    public Func<IServiceProvider, IJobStorage> JobStorageFactory { get; set; } = serviceProvider => ActivatorUtilities.CreateInstance<MemoryJobStorage>(serviceProvider);

    public JobManagerOptions Options { get; } = new();
}