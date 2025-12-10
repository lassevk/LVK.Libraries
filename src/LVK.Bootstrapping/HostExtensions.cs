using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

public static class HostExtensions
{
    extension<T>(T host)
        where T : IHost
    {
        public async Task InitializeAsync()
        {
            var moduleInitializers = host.Services.GetServices<IModuleInitializer>().ToList();
            foreach (IModuleInitializer initializer in moduleInitializers)
            {
                await initializer.InitializeAsync(CancellationToken.None);
            }

            var initializers = host.Services.GetServices<IHostInitializer<T>>().ToList();
            foreach (IHostInitializer<T> initializer in initializers)
            {
                await initializer.InitializeAsync(host);
            }
        }
    }
}