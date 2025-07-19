namespace LVK.Jobs;

public class JobLog
{
    public DateTimeOffset When { get; init; } = DateTimeOffset.UtcNow;
    public required string Line { get; init; }
}