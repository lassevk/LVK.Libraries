namespace LVK.Jobs;

internal class JobLogger : IJobLogger
{
    private readonly IJobStorage _jobStorage;
    private string? _jobId;

    public JobLogger(IJobStorage jobStorage)
    {
        _jobStorage = jobStorage ?? throw new ArgumentNullException(nameof(jobStorage));
    }

    public void SetJobId(string jobId) => _jobId = jobId;

    public async Task AddLogsAsync(IEnumerable<JobLog> items, CancellationToken cancellationToken)
    {
        if (_jobId == null)
        {
            throw new InvalidOperationException("Cannot add logs to a job without a job ID.");
        }

        await _jobStorage.AppendJobLogs(_jobId, items, cancellationToken);
    }
}