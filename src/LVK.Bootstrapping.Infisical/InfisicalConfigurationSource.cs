using Microsoft.Extensions.Configuration;

namespace LVK.Bootstrapping.Infisical;

public class InfisicalConfigurationSource : IConfigurationSource
{
    private readonly InfisicalOptions _options;

    public InfisicalConfigurationSource(InfisicalOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        return new InfisicalConfigurationProvider(_options);
    }
}