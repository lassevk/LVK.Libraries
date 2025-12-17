namespace LVK.FeatureFlags;

public class FeatureFlagsOptions
{
    public string SectionName { get; set; } = "FeatureFlags";
    public string? EnvironmentKey { get; set; }
    public string FlagsServerUrl { get; set; } = "https://flags.vkarlsen.no";

    public FeatureFlagsOptions UseSectionName(string sectionName)
    {
        SectionName = (sectionName ?? throw new ArgumentNullException(nameof(sectionName))).Trim();
        return this;
    }

    public FeatureFlagsOptions UseFlagsServer(string environmentKey, string? flagsServerUrl = null)
    {
        EnvironmentKey = (environmentKey ?? throw new ArgumentNullException(nameof(environmentKey))).Trim();
        FlagsServerUrl = flagsServerUrl ?? FlagsServerUrl;
        return this;
    }
}