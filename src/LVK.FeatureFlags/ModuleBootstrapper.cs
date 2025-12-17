using LVK.Bootstrapping;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LVK.FeatureFlags;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddFeatureFlags(options => builder.Configuration.GetSection("FeatureFlags").Bind(options));
    }
}