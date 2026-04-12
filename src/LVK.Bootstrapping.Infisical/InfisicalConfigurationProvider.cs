using Infisical.Sdk;
using Infisical.Sdk.Model;

using Microsoft.Extensions.Configuration;

namespace LVK.Bootstrapping.Infisical;

public class InfisicalConfigurationProvider(InfisicalOptions options) : ConfigurationProvider
{
    private readonly InfisicalOptions _options = options ?? throw new ArgumentNullException(nameof(options));

    public override void Load()
    {
        InfisicalSdkSettings settings = new InfisicalSdkSettingsBuilder()
           .WithHostUri(_options.HostUri!)
           .Build();

        var client = new InfisicalClient(settings);
        client.Auth().UniversalAuth().LoginAsync(_options.ClientId!,_options.ClientSecret!).ConfigureAwait(false).GetAwaiter().GetResult();

        var options = new ListSecretsOptions
        {
            SetSecretsAsEnvironmentVariables = false,
            SecretPath = "/",
            EnvironmentSlug = "prod",
            ProjectId = _options.ProjectId!,
        };

        Secret[] secrets = client.Secrets().ListAsync(options).ConfigureAwait(false).GetAwaiter().GetResult();

        var data = new Dictionary<string, string?>();
        foreach (Secret secret in secrets)
        {
            data.Add(secret.SecretKey.Replace("__", ":"), secret.SecretValue);
        }
        Data = data;
    }
}