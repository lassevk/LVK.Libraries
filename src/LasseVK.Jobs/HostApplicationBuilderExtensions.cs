using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Jobs;

public static class HostApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddMemoryJobStorage(this IHostApplicationBuilder builder)
    {
        _ = builder ?? throw new ArgumentNullException(nameof(builder));

        builder.Services.AddSingleton<IJobStorage, MemoryJobStorage>();

        return builder;
    }
}