using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("report")]
public class ReportDoneJob : Job
{
    public required WriteSidecarFileJob WritesidecarFile { get; init; }

    public override string ToString() => $"{base.ToString()} {WritesidecarFile.Checksum.File.FilePath}";
}