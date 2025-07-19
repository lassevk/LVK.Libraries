namespace LVK.Jobs;

public interface IJobHandler<in T> : IJobHandler
    where T : Job
{
    Task HandleAsync(T job, CancellationToken cancellationToken);
}

public interface IJobHandler
{
}