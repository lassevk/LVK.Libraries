namespace LVK.FeatureFlags;

public interface IFeatureFlags
{
    bool IsEnabled(string flagName);

    IFeatureFlagsScope CreateScope();
}