namespace LasseVK.Jobs;

public class JobManagerOptions
{
    public int CheckIntervalInSeconds { get; set; } = 1;

    public Dictionary<string, int?> MaxConcurrentJobs { get; set; } = new();
}