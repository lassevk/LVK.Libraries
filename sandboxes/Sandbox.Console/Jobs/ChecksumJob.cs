using System.Security.Cryptography;

using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("calculate-checksum")]
public class ChecksumJob : Job
{
    public required LoadFileJob File { get; init; }

    public byte[]? Checksum { get; set; }

    public override string ToString() => $"{base.ToString()} {File.FilePath}";
}

public class ChecksumJobHandler : IJobHandler<ChecksumJob>
{
    public Task HandleAsync(ChecksumJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("executing: " + job.Id);
        job.EnsureSuccess();

        byte[] checksum = SHA1.HashData(job.File.Contents!);
        job.Checksum = checksum;

        return Task.CompletedTask;
    }
}