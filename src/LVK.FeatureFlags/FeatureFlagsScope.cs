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

    public bool IsEnabled(string flagName)
    {
        ref bool isEnabled = ref CollectionsMarshal.GetValueRefOrAddDefault(_scopeCache, flagName, out bool exists);
        if (!exists)
        {
            isEnabled = _featureFlags.IsEnabled(flagName);
        }
        return isEnabled;
    }

    public IFeatureFlagsScope CreateScope() => _featureFlags.CreateScope();
}