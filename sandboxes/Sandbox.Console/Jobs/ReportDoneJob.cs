using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class ReportDoneJob : Job
{
    public required WriteSidecarFileJob WritesidecarFile { get; init; }
}

public class ReportDoneJobHandler : IJobHandler<ReportDoneJob>
{
    public Task HandleAsync(ReportDoneJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Checksum written for " + job.WritesidecarFile.Checksum.File.FilePath);
        return Task.CompletedTask;
    }
}