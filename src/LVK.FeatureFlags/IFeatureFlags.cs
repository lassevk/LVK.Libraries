namespace LVK.FeatureFlags;

public interface IFeatureFlags
{
    Task<bool> IsEnabled(string flagName);

    IFeatureFlagsScope CreateScope();
}