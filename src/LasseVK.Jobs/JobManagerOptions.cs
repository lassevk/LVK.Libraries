using JetBrains.Annotations;

namespace LasseVK.Jobs;

[PublicAPI]
public class JobManagerOptions
{
    public int CheckIntervalInSeconds { get; set; } = 1;

    public bool AllowUngroupedJobs { get; set; } = true;

    public Dictionary<string, int?> MaxConcurrentJobs { get; set; } = new();
}