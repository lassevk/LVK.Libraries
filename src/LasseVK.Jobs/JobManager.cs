using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LasseVK.Jobs;

internal class JobManager : IJobManager
{
    private readonly ILogger<JobManager> _logger;
    private readonly IJobStorage _jobStorage;
    private readonly IServiceProvider _serviceProvider;

    private readonly Dictionary<Type, List<PropertyInfo>> _dependencyProperties = new();

    public JobManager(ILogger<JobManager> logger, IJobStorage jobStorage, IServiceProvider serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jobStorage = jobStorage ?? throw new ArgumentNullException(nameof(jobStorage));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task SubmitAsync<T>(T job, CancellationToken cancellationToken)
        where T : Job
    {
        await SubmitAsync((Job)job, cancellationToken);
    }

    public async Task HandleAllJobsAsync(CancellationToken cancellationToken)
    {
        bool anyHandled = true;
        while (!cancellationToken.IsCancellationRequested)
        {
            if (!anyHandled)
            {
                await Task.Delay(1000, cancellationToken);
            }

            anyHandled = false;

            List<string> groupsWithPendingJobs = await _jobStorage.GetPendingJobGroupsAsync(cancellationToken);

            foreach (string group in groupsWithPendingJobs)
            {
                int roomInGroup = 100;
                if (group != "")
                {
                    _logger.LogInformation("Checking capacity of group {Group}", group);
                    JobGroup? groupConfiguration = await _jobStorage.GetJobGroupAsync(group, cancellationToken);
                    if (groupConfiguration is { MaxConcurrentJobs: > 0 })
                    {
                        int executingJobs = await _jobStorage.CountExecutingJobsInGroupAsync(group, cancellationToken);
                        if (executingJobs >= groupConfiguration.MaxConcurrentJobs)
                        {
                            _logger.LogInformation("Group {Group} is full, skipping for now", group);
                            continue;
                        }

                        roomInGroup = groupConfiguration.MaxConcurrentJobs.Value - executingJobs;
                        _logger.LogInformation("Group {Group} has {RoomInGroup} capacity available", group, roomInGroup);
                    }
                }

                List<Job> jobs = await _jobStorage.GetFirstPendingJobsInGroupAsync(group, roomInGroup, cancellationToken);

                _logger.LogInformation("Got {JobCount} jobs from group {Group}", jobs.Count, group);
                foreach (Job job in jobs)
                {
                    _logger.LogInformation("Handling job {Job}", job);
                    if (await _jobStorage.MarkAsExecuting(job.Id, cancellationToken))
                    {
                        anyHandled = true;
                        _ = HandleJobAsync(job, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }
    }

    private async Task HandleJobAsync(Job job, CancellationToken cancellationToken)
    {
        await Task.Yield();
        using IServiceScope serviceScope = _serviceProvider.CreateScope();

        Type handlerType = typeof(IJobHandler<>).MakeGenericType(job.GetType());
        try
        {
            _logger.LogInformation("Getting handler for job {Job}", job);
            object handler = serviceScope.ServiceProvider.GetRequiredService(handlerType);

            _logger.LogInformation("Executing job {Job}", job);
            await ((dynamic)handler).HandleAsync((dynamic)job, cancellationToken);
            _logger.LogInformation("Job {Job} completed successfully", job);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Job {Job} failed with an exception", job);
            job.Exception = new ExceptionSnapshot
            {
                Message = ex.Message,
                ExceptionType = ex.GetType().FullName ?? "<?>",
                StackTrace = ex.StackTrace ?? "<no stacktrace>",
            };
        }
        finally
        {
            _logger.LogInformation("Marking job {Job} as completed", job);
            await _jobStorage.MarkAsCompleted(job, cancellationToken);
        }
    }

    private async Task SubmitAsync(Job job, CancellationToken cancellationToken)
    {
        List<Job> dependencies = GetDependencies(job);

        foreach (Job dependency in dependencies)
        {
            await SubmitIfNeededAsync(dependency, cancellationToken);
        }

        _logger.LogInformation("Submitting job {Job}", job);
        await _jobStorage.QueueJobAsync(job, dependencies.Select(dependency => dependency.Id), cancellationToken);
    }

    private async Task SubmitIfNeededAsync(Job job, CancellationToken cancellationToken)
    {
        bool jobExists = await _jobStorage.JobExistsAsync(job.Id, cancellationToken);

        if (!jobExists)
        {
            await SubmitAsync(job, cancellationToken);
        }
    }

    private List<Job> GetDependencies(Job job)
    {
        var result = new List<Job>();
        List<PropertyInfo> properties = GetDependencyProperties(job.GetType());

        foreach (PropertyInfo property in properties)
        {
            if (property.GetValue(job) is Job jobValue)
            {
                result.Add(jobValue);
            }
        }

        return result;
    }

    private List<PropertyInfo> GetDependencyProperties(Type type)
    {
        if (_dependencyProperties.TryGetValue(type, out List<PropertyInfo>? result))
        {
            return result;
        }

        result = new List<PropertyInfo>();

        foreach (PropertyInfo property in type.GetProperties())
        {
            if (property is not { CanRead: true, CanWrite: true })
            {
                continue;
            }

            if (property.GetIndexParameters().Length > 0)
            {
                continue;
            }

            if (!property.PropertyType.IsAssignableTo(typeof(Job)))
            {
                continue;
            }

            result.Add(property);
        }

        _dependencyProperties[type] = result;
        return result;
    }

    public async Task ConfigureGroupAsync(string groupName, Action<JobGroup> configure, CancellationToken cancellationToken)
    {
        JobGroup group = await _jobStorage.GetJobGroupAsync(groupName, cancellationToken) ?? new JobGroup { Name = groupName };
        configure(group);
        await _jobStorage.SetJobGroupAsync(group, cancellationToken);
    }

    public async Task<JobGroup?> GetConfiguratinAsync(string groupName, CancellationToken cancellationToken) => await _jobStorage.GetJobGroupAsync(groupName, cancellationToken);
}