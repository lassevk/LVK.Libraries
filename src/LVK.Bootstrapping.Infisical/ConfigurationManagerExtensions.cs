using LVK.Bootstrapping.Infisical.Api;
using LVK.Bootstrapping.Infisical.Configuration;
using LVK.Bootstrapping.Infisical.Refresh;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping.Infisical;

public static class HostApplicationBuilderExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public async Task<IHostApplicationBuilder> AddInfisical(string configurationKey = "Infisical", Action<InfisicalOptions>? configure = null)
        {
            var options = new InfisicalOptions();
            builder.Configuration.GetSection(configurationKey).Bind(options);
            configure?.Invoke(options);

            if (!options.Validate())
            {
                return builder;
            }

            var service = new InfisicalService(options);
            Secret[] secrets = await service.GetSecretsAsync();

            InfisicalRefreshChannel? channel = null;
            if (options.RefreshIntervalSeconds.HasValue)
            {
                builder.Services.Configure<InfisicalRefreshServiceOptions>(opt =>
                {
                    opt.RefreshIntervalSeconds = options.RefreshIntervalSeconds.Value;
                    opt.Secrets = secrets;
                    opt.Service = service;
                });

                builder.Services.AddHostedService<InfisicalRefreshService>();
                channel = new();
                builder.Services.AddSingleton<InfisicalRefreshChannel>(channel);
            }

            var newSource = new InfisicalConfigurationSource(secrets, channel);
            bool addedSource = false;
            for (int index = 0; index < builder.Configuration.Sources.Count; index++)
            {
                IConfigurationSource source = builder.Configuration.Sources[index];
                if (source is EnvironmentVariablesConfigurationSource { Prefix: null })
                {
                    builder.Configuration.Sources.Insert(index, newSource);
                    addedSource = true;
                    break;
                }
            }

            if (!addedSource)
            {
                builder.Configuration.Sources.Add(newSource);
            }

            return builder;
        }
    }
}