using System.Text;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace LVK.FeatureFlags;

internal class FeatureFlags : IFeatureFlags
{
    private readonly IConfiguration _configuration;
    private readonly IOptions<FeatureFlagsOptions> _options;

    public FeatureFlags(IConfiguration configuration, IOptions<FeatureFlagsOptions> options)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public async Task<bool> IsEnabled(string flagName)
    {
        bool isLocallyEnabled = GetConfigurationFlag(flagName);
        if (isLocallyEnabled)
        {
            return true;
        }

        return false;
    }

    private bool GetConfigurationFlag(string flagName)
    {
        string featureFlagPath = CreateKey(flagName);
        return _configuration.GetValue(featureFlagPath, false);
    }

    public IFeatureFlagsScope CreateScope() => new FeatureFlagsScope(this);

    private string CreateKey(string flagName)
    {
        const string defaultPrefix = "FeatureFlags/";
        var builder = new StringBuilder(flagName + defaultPrefix.Length * 2);
        builder.Append(!string.IsNullOrWhiteSpace(_options.Value.SectionName) ? _options.Value.SectionName : defaultPrefix);
        if (builder.Length == 0)
        {
            builder.Append(defaultPrefix);
        }

        if (builder[^1] != '/')
        {
            builder.Append('/');
        }

        builder.Replace('/', ':');
        builder.Append(flagName);
        return builder.ToString();
    }
}