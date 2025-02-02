using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("calculate-checksum")]
public class ChecksumJob : Job
{
    public required LoadFileJob File { get; init; }

    public byte[]? Checksum { get; set; }

    public override string ToString() => $"{base.ToString()} {File.FilePath}";
}