using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("sidecar")]
public class WriteSidecarFileJob : Job
{
    public required ChecksumJob Checksum { get; init; }

    public override string ToString() => $"{base.ToString()} {Checksum.File.FilePath}";
}