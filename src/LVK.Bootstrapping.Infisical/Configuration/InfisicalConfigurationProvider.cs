using Microsoft.Extensions.Configuration;

namespace LVK.Bootstrapping.Infisical.Configuration;

internal class InfisicalConfigurationProvider : ConfigurationProvider
{
    private Secret[] _secrets;

    public InfisicalConfigurationProvider(Secret[] secrets)
    {
        _secrets = secrets ?? throw new ArgumentNullException(nameof(secrets));
    }

    public override void Load()
    {
        var data = new Dictionary<string, string?>();
        foreach (Secret secret in _secrets)
        {
            data.Add(secret.SecretKey.Replace("__", ":"), secret.SecretValue);
        }
        Data = data;
    }

    public void Update(Secret[] secrets)
    {
        _secrets = secrets;
        Load();
        OnReload();
    }
}