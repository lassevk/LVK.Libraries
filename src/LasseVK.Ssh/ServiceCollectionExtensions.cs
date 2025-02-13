using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LasseVK.Ssh;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSshProxy(this IServiceCollection services, Action<SshProxyOptions> configureOptions)
    {
        var options = new SshProxyOptions();
        configureOptions(options);
        options.Validate();

        services.AddSingleton<IOptions<SshProxyOptions>>(new OptionsWrapper<SshProxyOptions>(options));
        services.AddHostedService<SshProxyService>();
        services.AddSingleton(new SshAsyncInitialization());
        services.AddSingleton<IAsyncInitialization>(sp => sp.GetRequiredService<SshAsyncInitialization>());

        return services;
    }
}