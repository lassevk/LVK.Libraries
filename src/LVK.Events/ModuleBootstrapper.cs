using LVK.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Events;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public Task BootstrapAsync(IHostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IEventBus, EventBus>();

        return Task.CompletedTask;
    }
}