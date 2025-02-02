namespace LasseVK.Jobs;

public interface IJobManager
{
    Task SubmitAsync<T>(T job, CancellationToken cancellationToken = default)
        where T : Job;

    Task HandleAllJobsAsync(CancellationToken cancellationToken);

    Task ConfigureGroupAsync(string groupName, Action<JobGroup> configure, CancellationToken cancellationToken);
    Task<JobGroup?> GetConfiguratinAsync(string groupName, CancellationToken cancellationToken);
}