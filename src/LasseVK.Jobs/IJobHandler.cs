namespace LasseVK.Jobs;

public interface IJobHandler<in T>
{
    Task HandleAsync(T job, CancellationToken cancellationToken);
}