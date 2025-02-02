namespace LasseVK.Jobs;

public class JobGroup
{
    public required string Name { get; init; }
    public int? MaxConcurrentJobs { get; set; }
}