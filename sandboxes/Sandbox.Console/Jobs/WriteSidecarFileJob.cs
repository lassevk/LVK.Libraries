using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class WriteSidecarFileJob : Job
{
    public required ChecksumJob Checksum { get; init; }

    public override string ToString() => $"{base.ToString()} {Checksum.File.FilePath}";
}

public class WriteSidecarFileJobHandler : IJobHandler<WriteSidecarFileJob>
{
    private readonly IJobManager _jobManager;

    public WriteSidecarFileJobHandler(IJobManager jobManager)
    {
        _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
    }

    public async Task HandleAsync(WriteSidecarFileJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("executing: " + job.Id);
        job.EnsureSuccess();

        string filePath = Path.ChangeExtension(job.Checksum.File.FilePath, ".checksum");
        await File.WriteAllBytesAsync(filePath, job.Checksum.Checksum!, cancellationToken);

        await _jobManager.SubmitAsync(new ReportDoneJob { WritesidecarFile = job }, cancellationToken);
    }
}