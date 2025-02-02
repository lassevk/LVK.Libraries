using System.Security.Cryptography;

using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class ChecksumJobHandler : IJobHandler<ChecksumJob>
{
    public Task HandleAsync(ChecksumJob job, CancellationToken cancellationToken)
    {
        byte[] checksum = SHA1.HashData(job.File.Contents!);
        job.Checksum = checksum;

        return Task.CompletedTask;
    }
}