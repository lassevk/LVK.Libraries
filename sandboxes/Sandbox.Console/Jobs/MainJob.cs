using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("main")]
public class MainJob : Job
{
    public List<DependencyJob> Dependencies { get; } = new();
}

public class MainJobHandler : IJobHandler<MainJob>
{
    public Task HandleAsync(MainJob job, CancellationToken cancellationToken)
    {
        foreach (DependencyJob dependency in job.Dependencies)
        {
            System.Console.WriteLine("MainJob: " + dependency);
        }

        return Task.CompletedTask;
    }
}