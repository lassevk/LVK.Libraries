using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class DependencyJob : Job
{
    public required int Counter { get; init; }
}

public class DependencyJobHandler : IJobHandler<DependencyJob>
{
    public Task HandleAsync(DependencyJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine($"Dependency Job: {job.Counter}");
        return Task.CompletedTask;
    }
}