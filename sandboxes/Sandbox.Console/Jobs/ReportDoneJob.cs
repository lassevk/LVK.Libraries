using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("report")]
public class ReportDoneJob : Job
{
    public required WriteSidecarFileJob WritesidecarFile { get; init; }

    public override string ToString() => $"{base.ToString()} {WritesidecarFile.Checksum.File.FilePath}";
}

public class ReportDoneJobHandler : IJobHandler<ReportDoneJob>
{
    public Task HandleAsync(ReportDoneJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Checksum written for " + job.WritesidecarFile.Checksum.File.FilePath);
        return Task.CompletedTask;
    }
}