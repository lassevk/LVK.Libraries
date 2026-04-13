using LVK.Bootstrapping.Infisical.Refresh;

using Microsoft.Extensions.Configuration;

namespace LVK.Bootstrapping.Infisical.Configuration;

internal class InfisicalConfigurationSource : IConfigurationSource
{
    private readonly Secret[] _secrets;
    private readonly InfisicalRefreshChannel? _refreshChannel;

    public InfisicalConfigurationSource(Secret[] secrets, InfisicalRefreshChannel? refreshChannel)
    {
        _secrets = secrets ?? throw new ArgumentNullException(nameof(secrets));
        _refreshChannel = refreshChannel;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        var provider = new InfisicalConfigurationProvider(_secrets);
        _refreshChannel?.Provider = provider;
        return provider;
    }
}