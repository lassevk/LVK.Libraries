using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class ReportDoneJobHandler : IJobHandler<ReportDoneJob>
{
    public Task HandleAsync(ReportDoneJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Checksum written for " + job.WritesidecarFile.Checksum.File.FilePath);
        return Task.CompletedTask;
    }
}