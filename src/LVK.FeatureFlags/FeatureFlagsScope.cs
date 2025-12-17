using System.Runtime.InteropServices;

namespace LVK.FeatureFlags;

internal class FeatureFlagsScope : IFeatureFlagsScope
{
    private readonly IFeatureFlags _featureFlags;
    private readonly Dictionary<string, bool> _scopeCache = new();

    internal FeatureFlagsScope(IFeatureFlags featureFlags)
    {
        _featureFlags = featureFlags;
    }

    public async Task<bool> IsEnabled(string flagName)
    {
        if (!_scopeCache.TryGetValue(flagName, out bool value))
        {
            value = _scopeCache[flagName] = await _featureFlags.IsEnabled(flagName);
        }

        return value;
    }

    public IFeatureFlagsScope CreateScope() => _featureFlags.CreateScope();
}