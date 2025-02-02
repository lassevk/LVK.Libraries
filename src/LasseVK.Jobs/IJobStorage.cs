namespace LasseVK.Jobs;

public interface IJobStorage
{
    Task<bool> JobExistsAsync(string id, CancellationToken cancellationToken);
    Task QueueJobAsync(Job job, IEnumerable<string> dependsOnJobIds, CancellationToken cancellationToken);
    Task<List<string>> GetPendingJobGroupsAsync(CancellationToken cancellationToken);
    Task<Job?> GetFirstPendingJobInGroupAsync(string group, CancellationToken cancellationToken);
    Task MarkAsCompleted(Job job, CancellationToken cancellationToken);
    Task<bool> MarkAsExecuting(string id, CancellationToken cancellationToken);

    Task<JobGroup?> GetJobGroupAsync(string groupName, CancellationToken cancellationToken);
    Task SetJobGroupAsync(JobGroup group, CancellationToken cancellationToken);
}