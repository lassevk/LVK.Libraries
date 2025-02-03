using LasseVK.Bootstrapping.ConsoleApplications;
using LasseVK.Jobs;

using Sandbox.Console.Jobs;

namespace Sandbox.Console;

public class Application : IConsoleApplication
{
    private readonly IJobManager _jobManager;

    public Application(IJobManager jobManager)
    {
        _jobManager = jobManager;
    }

    public async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        var job = new MainJob();
        job.Dependencies.Add(new DependencyJob
        {
            Counter = 1,
        });
        job.Dependencies.Add(new DependencyJob
        {
            Counter = 2,
        });

        await _jobManager.SubmitAsync(job, cancellationToken);
        await _jobManager.HandleAllJobsAsync(cancellationToken);

        return 0;
    }
}