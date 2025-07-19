using Microsoft.EntityFrameworkCore;

namespace LVK.Jobs.PostgreSQL;

internal class PostgresJobStorage : IJobStorage
{
    private readonly IDbContextFactory<PostgresDbContext> _dbContextFactory;
    private readonly Lock _migrationLock = new();
    private bool _isMigrated;

    public PostgresJobStorage(IDbContextFactory<PostgresDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
    }

    private async Task<PostgresDbContext> CreateDbContextAsync(CancellationToken cancellationToken)
    {
        PostgresDbContext dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        bool isFirstTime = false;
        lock (_migrationLock)
        {
            if (!_isMigrated)
            {
                _isMigrated = true;
                dbContext.Database.Migrate();

                isFirstTime = true;
            }
        }

        if (isFirstTime)
        {
            dbContext.JobLogs!.RemoveRange(dbContext.JobLogs);
            dbContext.Jobs!.RemoveRange(dbContext.Jobs);
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return dbContext;
    }

    public async Task<bool> JobExistsAsync(string id, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);
        string? existingId = await dbContext.Jobs!.Where(x => x.Id == id).Select(job => job.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return existingId != null;
    }

    public async Task QueueJobAsync(Job job, IEnumerable<string> dependsOnJobIds, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);

        SerializedJob serialized = JobSerializer.Serialize(job);

        List<JobEntity> dependencies = await dbContext.Jobs!.Where(x => dependsOnJobIds.Contains(x.Id)).ToListAsync(cancellationToken);

        var entity = new JobEntity
        {
            Id = job.Id,
            Group = serialized.Group,
            Json = serialized.Json,
            DependsOn = dependencies,
            Status = JobStatus.Queued, };

        dbContext.Jobs!.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<string>> GetPendingJobGroupsAsync(CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);

        List<string> groups = await dbContext.Jobs!.Where(job => job.Status == JobStatus.Queued)
           .Where(job => job.DependsOn!.All(dependency => dependency.Status == JobStatus.Completed))
           .Select(job => job.Group ?? "")
           .Distinct()
           .ToListAsync(cancellationToken);

        return groups;
    }

    public async Task<List<Job>> GetFirstPendingJobsInGroupAsync(string group, int amount, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);

        List<JobEntity> entities = await dbContext.Jobs!.Where(job => (job.Group ?? "") == group && job.Status == JobStatus.Queued)
           .Where(job => job.DependsOn!.All(dependency => dependency.Status == JobStatus.Completed))
           .OrderBy(job => job.Id)
           .Take(amount)
           .ToListAsync(cancellationToken);

        var result = new List<Job>();
        foreach (JobEntity entity in entities)
        {
            Job? job = await HydrateEnvelopeToJobAsync(dbContext, entity, cancellationToken);
            if (job is not null)
            {
                result.Add(job);
            }
        }

        return result;
    }

    private async Task<Job?> HydrateEnvelopeToJobAsync(PostgresDbContext dbContext, JobEntity entity, CancellationToken cancellationToken)
    {
        var serialized = new SerializedJob
        {
            Group = entity.Group ?? "", Json = entity.Json,
        };

        Job job = JobSerializer.Deserialize(serialized, entity.Id);

        List<JobEntity> dependencyEntities = await dbContext.Jobs!.Where(dependency => dependency.DependsOnMe!.Contains(entity)).ToListAsync(cancellationToken);

        if (dependencyEntities.Count > 0)
        {
            var dependencies = new List<Job>();
            foreach (JobEntity dependencyEntity in dependencyEntities)
            {
                Job? depencency = await HydrateEnvelopeToJobAsync(dbContext, dependencyEntity, cancellationToken);
                if (depencency is null)
                {
                    throw new InvalidOperationException("Dependent job not found");
                }

                dependencies.Add(depencency);
            }

            JobSerializer.PopulateDependencies(job, dependencies);
        }

        return job;
    }


    public async Task MarkAsCompleted(Job job, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);
        JobEntity? entity = await dbContext.Jobs!.Where(x => x.Id == job.Id).FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
        {
            throw new InvalidOperationException("Job not found");
        }

        SerializedJob serialized = JobSerializer.Serialize(job);
        entity.Json = serialized.Json;
        entity.Status = JobStatus.Completed;
        entity.WhenCompleted = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> MarkAsExecuting(string id, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);
        int result = await dbContext.Jobs!.Where(job => job.Id == id && job.Status == JobStatus.Queued)
           .ExecuteUpdateAsync(entity => entity.SetProperty(x => x.Status, JobStatus.Executing).SetProperty(x => x.WhenStarted, DateTimeOffset.UtcNow), cancellationToken);

        return result > 0;
    }

    public async Task<int> CountExecutingJobsInGroupAsync(string group, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);

        return await dbContext.Jobs!.Where(job => job.Group == group && job.Status == JobStatus.Executing).CountAsync(cancellationToken);
    }

    public async Task AppendJobLogs(string jobId, IEnumerable<JobLog> items, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);
        foreach (JobLog item in items)
        {
            dbContext.JobLogs!.Add(new JobLogEntity
            {
                JobId = jobId, WhenLogged = item.When, Line = item.Line,
            });
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<JobLog>> GetJobLogsAsync(string jobId, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);

        List<JobLogEntity> logEntities = await dbContext.JobLogs!.Where(log => log.JobId == jobId).ToListAsync(cancellationToken);
        return logEntities.Select(entity => new JobLog
            {
                When = entity.WhenLogged, Line = entity.Line,
            })
           .ToList();
    }
}