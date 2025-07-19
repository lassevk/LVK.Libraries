namespace LVK.Jobs;

public class ExceptionSnapshot
{
    public required string ExceptionType { get; init; }
    public required string Message { get; init; }
    public required string StackTrace { get; init; }
}