using Microsoft.EntityFrameworkCore;

namespace LasseVK.Jobs.PostgreSQL;

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
        lock (_migrationLock)
        {
            if (!_isMigrated)
            {
                _isMigrated = true;
                dbContext.Database.Migrate();
            }
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
            Identifier = serialized.Identifier,
            Group = serialized.Group,
            JobJson = serialized.Json,
            DependsOn = dependencies,
            Status = JobStatus.Queued, };

        // foreach (JobEntity dependency in dependencies)
        // {
        //     dependency.DependsOnMe ??= [];
        //     dependency.Dependents.Add(entity);
        // }
        //
        dbContext.Jobs!.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<string>> GetPendingJobGroupsAsync(CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);

        List<string> groups = await dbContext.Jobs!.Where(job => job.Status == JobStatus.Queued).Where(job => job.DependsOn!.All(dependency => dependency.Status == JobStatus.Completed))
           .Select(job => job.Group ?? "").Distinct().ToListAsync(cancellationToken);

        return groups;
    }

    public async Task<Job?> GetFirstPendingJobInGroupAsync(string group, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);

        JobEntity? entity = await dbContext.Jobs!.Include(entity => entity.DependsOn).Where(job => (job.Group ?? "") == group && job.Status == JobStatus.Queued)
           .Where(job => job.DependsOn!.All(dependency => dependency.Status == JobStatus.Completed)).OrderBy(job => job.Id).FirstOrDefaultAsync(cancellationToken);

        Job? job = await HydrateEnvelopeToJobAsync(dbContext, entity!, cancellationToken);
        return job;
    }

    private async Task<Job?> HydrateEnvelopeToJobAsync(PostgresDbContext dbContext, JobEntity entity, CancellationToken cancellationToken)
    {
        var serialized = new SerializedJob
        {
            Identifier = entity.Identifier,
            Group = entity.Group ?? "",
            Json = entity.JobJson,
        };
        Job job = JobSerializer.Deserialize(serialized, entity.Id);

        if (entity.DependsOn?.Count > 0)
        {
            var dependencies = new List<Job>();
            foreach (JobEntity dependencyEntity in entity.DependsOn)
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
        entity.JobJson = serialized.Json;
        entity.Status = JobStatus.Completed;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> MarkAsExecuting(string id, CancellationToken cancellationToken)
    {
        await using PostgresDbContext dbContext = await CreateDbContextAsync(cancellationToken);
        int result = await dbContext.Jobs!.Where(job => job.Id == id && job.Status == JobStatus.Queued).ExecuteUpdateAsync(entity => entity.SetProperty(x => x.Status, JobStatus.Executing), cancellationToken);
        return result > 0;
    }
}