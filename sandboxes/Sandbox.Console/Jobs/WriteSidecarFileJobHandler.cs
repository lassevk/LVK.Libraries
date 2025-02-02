using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class WriteSidecarFileJobHandler : IJobHandler<WriteSidecarFileJob>
{
    private readonly IJobManager _jobManager;

    public WriteSidecarFileJobHandler(IJobManager jobManager)
    {
        _jobManager = jobManager ?? throw new ArgumentNullException(nameof(jobManager));
    }

    public async Task HandleAsync(WriteSidecarFileJob job, CancellationToken cancellationToken)
    {
        string filePath = Path.ChangeExtension(job.Checksum.File.FilePath, ".checksum");
        await File.WriteAllBytesAsync(filePath, job.Checksum.Checksum!, cancellationToken);

        await _jobManager.SubmitAsync(new ReportDoneJob { WritesidecarFile = job }, cancellationToken);
    }
}