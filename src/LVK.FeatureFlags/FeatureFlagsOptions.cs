namespace LVK.FeatureFlags;

public class FeatureFlagsOptions
{
    public string SectionName { get; set; } = "FeatureFlags";

    public FeatureFlagsOptions UseSectionName(string sectionName)
    {
        SectionName = (sectionName ?? throw new ArgumentNullException(nameof(sectionName))).Trim();
        return this;
    }
}