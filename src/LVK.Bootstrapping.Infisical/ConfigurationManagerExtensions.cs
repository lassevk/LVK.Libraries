using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping.Infisical;

public static class HostApplicationBuilderExtensions
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddInfisical(string configurationKey = "Infisical", Action<InfisicalOptions>? configure = null)
        {
            var options = new InfisicalOptions();
            builder.Configuration.GetSection(configurationKey).Bind(options);
            configure?.Invoke(options);

            if (!options.Validate())
            {
                return builder;
            }

            var newSource = new InfisicalConfigurationSource(options);
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