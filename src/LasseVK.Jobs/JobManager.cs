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
        while (true)
        {
            List<string> groupsWithPendingJobs = await _jobStorage.GetPendingJobGroupsAsync(cancellationToken);

            foreach (string group in groupsWithPendingJobs)
            {
                // TODO: Check if group is full

                Job? job = await _jobStorage.GetFirstPendingJobInGroupAsync(group, cancellationToken);
                if (job is null)
                {
                    continue;
                }

                _logger.LogInformation("Handling job {Job}", job);
                if (await _jobStorage.MarkAsExecuting(job.Id, cancellationToken))
                {
                    _ = HandleJobAsync(job, cancellationToken).ConfigureAwait(false);
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
}