using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Ssh;

public static class HostApplicationBuilderExtensions
{
    public static HostApplicationBuilder AddSshProxy(this HostApplicationBuilder builder)
    {
        builder.Services.AddSshProxy(configure =>
        {
            builder.Configuration.GetSection(SshProxyOptions.ConfigurationKey).Bind(configure);
        });
        return builder;
    }
}