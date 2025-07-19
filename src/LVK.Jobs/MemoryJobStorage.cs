using System.Collections.Concurrent;

using Microsoft.Extensions.Logging;

namespace LVK.Jobs;

internal class MemoryJobStorage : IJobStorage
{
    private readonly ILogger<MemoryJobStorage> _logger;

    private readonly ConcurrentDictionary<string, MemoryJobStorageEnvelope> _jobsById = new();
    private readonly ConcurrentDictionary<string, List<string>> _forwardJobDependencies = new();
    private readonly ConcurrentDictionary<string, List<string>> _backwardJobDependencies = new();

    private readonly ConcurrentDictionary<string, List<JobLog>> _jobLogsByJobId = new();

    public MemoryJobStorage(ILogger<MemoryJobStorage> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task<bool> JobExistsAsync(string id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Checking if job {Id} exists", id);
        bool jobExists = _jobsById.ContainsKey(id);
        _logger.LogInformation("Job {Id} exists = {Exists}", id, jobExists);

        return Task.FromResult(jobExists);
    }

    public Task QueueJobAsync(Job job, IEnumerable<string> dependsOnJobIds, CancellationToken cancellationToken)
    {
        var depList = dependsOnJobIds.ToList();

        _logger.LogInformation("Queuing job {Id} with dependencies {Dependencies}", job.Id, depList);

        SerializedJob serialized = JobSerializer.Serialize(job);
        if (!_jobsById.TryAdd(job.Id, new MemoryJobStorageEnvelope
        {
            Id = job.Id, Job = serialized, Dependencies = depList,
        }))
        {
            throw new InvalidOperationException("Failed to add job");
        }

        foreach (string dependency in depList)
        {
            addIfNeeded(_forwardJobDependencies, dependency, job.Id);
            addIfNeeded(_backwardJobDependencies, job.Id, dependency);
        }

        return Task.CompletedTask;

        void addIfNeeded(ConcurrentDictionary<string, List<string>> dict, string key, string value)
        {
            List<string> list = dict.GetOrAdd(key, _ => new List<string>());
            list.Add(value);
        }
    }

    public Task MarkAsCompleted(Job job, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Marking job {Id} as completed", job.Id);
        if (!_jobsById.TryGetValue(job.Id, out MemoryJobStorageEnvelope? envelope))
        {
            throw new InvalidOperationException($"Job {job.Id} does not exist");
        }

        SerializedJob serialized = JobSerializer.Serialize(job);
        envelope.Job = serialized;
        envelope.Status = JobStatus.Completed;
        return Task.CompletedTask;
    }

    public Task<bool> MarkAsExecuting(string id, CancellationToken cancellationToken)
    {
        if (!_jobsById.TryGetValue(id, out MemoryJobStorageEnvelope? envelope))
        {
            return Task.FromResult(false);
        }

        envelope.Status = JobStatus.Executing;
        return Task.FromResult(true);
    }

    public Task<int> CountExecutingJobsInGroupAsync(string group, CancellationToken cancellationToken)
    {
        return Task.FromResult(_jobsById.Values.Count(envelope => envelope.Status == JobStatus.Executing && envelope.Job.Group == group));
    }

    public Task AppendJobLogs(string jobId, IEnumerable<JobLog> items, CancellationToken cancellationToken)
    {
        List<JobLog> list = _jobLogsByJobId.GetOrAdd(jobId, new List<JobLog>());
        lock (list)
        {
            list.AddRange(items);
        }

        return Task.CompletedTask;
    }

    public Task<List<JobLog>> GetJobLogsAsync(string jobId, CancellationToken cancellationToken)
    {
        _jobLogsByJobId.TryGetValue(jobId, out List<JobLog>? logs);
        return Task.FromResult(logs ?? []);
    }

    public Task<List<string>> GetPendingJobGroupsAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(GetPendingJobEnvelopes().Select(envelope => envelope.Job.Group).Distinct().ToList());
    }

    private IEnumerable<MemoryJobStorageEnvelope> GetPendingJobEnvelopes()
    {
        foreach (MemoryJobStorageEnvelope envelope in _jobsById.Values)
        {
            if (envelope.Status != JobStatus.Queued)
            {
                continue;
            }

            if (_backwardJobDependencies.TryGetValue(envelope.Id, out List<string>? dependencies))
            {
                if (dependencies.Any(dependency => _jobsById[dependency].Status != JobStatus.Completed))
                {
                    continue;
                }
            }

            yield return envelope;
        }
    }

    public async Task<List<Job>> GetFirstPendingJobsInGroupAsync(string group, int amount, CancellationToken cancellationToken)
    {
        var envelopes = GetPendingJobEnvelopes().Where(envelope => envelope.Job.Group == group && envelope.Status == JobStatus.Queued).OrderBy(envelope => envelope.Id).Take(amount).ToList();

        var result = new List<Job>();
        foreach (MemoryJobStorageEnvelope envelope in envelopes)
        {
            Job? job = await HydrateEnvelopeToJobAsync(envelope, cancellationToken);
            if (job is not null)
            {
                result.Add(job);
            }
        }

        return result;
    }

    private async Task<Job?> HydrateEnvelopeToJobAsync(MemoryJobStorageEnvelope envelope, CancellationToken cancellationToken)
    {
        Job job = JobSerializer.Deserialize(envelope.Job, envelope.Id);

        var dependencies = new List<Job>();
        foreach (string dependentJobId in envelope.Dependencies)
        {
            Job? dependentJob = await GetJobByIdAsync(dependentJobId, cancellationToken);
            if (dependentJob is not null)
            {
                dependencies.Add(dependentJob);
            }
        }

        if (dependencies.Count > 0)
        {
            JobSerializer.PopulateDependencies(job, dependencies);
        }

        return job;
    }

    private async Task<Job?> GetJobByIdAsync(string id, CancellationToken cancellationToken)
    {
        if (!_jobsById.TryGetValue(id, out MemoryJobStorageEnvelope? envelope))
        {
            return null;
        }

        return await HydrateEnvelopeToJobAsync(envelope, cancellationToken);
    }

    public Task SetJobStatusAsync(string id, JobStatus status, CancellationToken cancellationToken)
    {
        if (!_jobsById.TryGetValue(id, out MemoryJobStorageEnvelope? envelope))
        {
            throw new InvalidOperationException($"Job {id} does not exist");
        }

        envelope.Status = status;
        return Task.CompletedTask;
    }
}