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

        var sumJob = new CalculateSumJob
        {
            Operand1 = new CalculateOperand1Job(),
            Operand2 = new CalculateOperand2Job(),
        };

        await _jobManager.SubmitAsync(sumJob, cancellationToken);

        await _jobManager.HandleAllJobsAsync(cancellationToken);

        return 0;
    }
}