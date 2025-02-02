namespace LasseVK.Jobs;

internal class JobGroups : IJobGroups
{
    private readonly IJobStorage _jobStorage;

    public JobGroups(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage ?? throw new ArgumentNullException(nameof(jobStorage));
    }

    public async Task ConfigureGroupAsync(string groupName, Action<JobGroup> configure, CancellationToken cancellationToken)
    {
        JobGroup group = await _jobStorage.GetJobGroupAsync(groupName, cancellationToken) ?? new JobGroup { Name = groupName };
        configure(group);
        await _jobStorage.SetJobGroupAsync(group, cancellationToken);
    }

    public async Task<JobGroup?> GetConfiguratinAsync(string groupName, CancellationToken cancellationToken) => await _jobStorage.GetJobGroupAsync(groupName, cancellationToken);
}