namespace LasseVK.Jobs;

public interface IJobGroups
{
    Task ConfigureGroupAsync(string groupName, Action<JobGroup> configure, CancellationToken cancellationToken);
    Task<JobGroup?> GetConfiguratinAsync(string groupName, CancellationToken cancellationToken);
}