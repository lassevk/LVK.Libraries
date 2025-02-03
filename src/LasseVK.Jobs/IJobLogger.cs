namespace LasseVK.Jobs;

public interface IJobLogger
{
    async Task AddLogAsync(string line, CancellationToken cancellationToken = default)
        => await AddLogsAsync([
            new JobLog
            {
                Line = line,
            },
        ], cancellationToken);

    Task AddLogsAsync(IEnumerable<JobLog> items, CancellationToken cancellationToken);
}