namespace LasseVK.Jobs;

public interface IJobManager
{
    Task SubmitAsync<T>(T job, CancellationToken cancellationToken = default)
        where T : Job;

    Task HandleAllJobsAsync(CancellationToken cancellationToken);

    Task<List<JobLog>> GetJobLogsAsync(string jobId, CancellationToken cancellationToken = default);
}