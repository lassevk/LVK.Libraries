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
        await Task.Yield();

        foreach (string file in Directory.EnumerateFiles(@"D:\Temp", "*.*", SearchOption.AllDirectories))
        {
            var load = new LoadFileJob { FilePath = file };
            var checksum = new ChecksumJob { File = load };
            var writeSidecarJob = new WriteSidecarFileJob { Checksum = checksum };

            await _jobManager.SubmitAsync(writeSidecarJob, cancellationToken);
        }

        await _jobManager.HandleAllJobsAsync(cancellationToken);

        return 0;
    }
}