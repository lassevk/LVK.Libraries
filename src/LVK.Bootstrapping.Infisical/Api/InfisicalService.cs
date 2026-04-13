using Infisical.Sdk;
using Infisical.Sdk.Model;

using LVK.Bootstrapping.Infisical.Configuration;

namespace LVK.Bootstrapping.Infisical.Api;

internal class InfisicalService
{
    private readonly InfisicalOptions _options;

    public InfisicalService(InfisicalOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<Secret[]> GetSecretsAsync()
    {
        InfisicalSdkSettings settings = new InfisicalSdkSettingsBuilder()
           .WithHostUri(_options.HostUri!)
           .Build();

        var client = new InfisicalClient(settings);
        await client.Auth().UniversalAuth().LoginAsync(_options.ClientId!, _options.ClientSecret!).ConfigureAwait(false);

        var options = new ListSecretsOptions
        {
            SetSecretsAsEnvironmentVariables = false,
            SecretPath = _options.SecretPath,
            EnvironmentSlug = _options.Environment,
            ProjectId = _options.ProjectId!,
        };

        Secret[] secrets = await client.Secrets().ListAsync(options).ConfigureAwait(false);
        return secrets;
    }
}