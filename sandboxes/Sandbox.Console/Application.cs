using LasseVK.Bootstrapping.ConsoleApplications;
using LasseVK.Jobs;
using LasseVK.Pushover;

using Sandbox.Console.Jobs;

namespace Sandbox.Console;

public class Application : IConsoleApplication
{
    private readonly IJobManager _jobManager;
    private readonly IPushover _pushover;

    public Application(IJobManager jobManager, IPushover pushover)
    {
        _jobManager = jobManager;
        _pushover = pushover;
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

        await _pushover.SendAsync("Jobs submitted", cancellationToken);
        await _jobManager.HandleAllJobsAsync(cancellationToken);

        return 0;
    }
}