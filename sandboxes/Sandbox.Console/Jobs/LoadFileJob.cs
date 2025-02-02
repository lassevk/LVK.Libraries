using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("load-file")]
public class LoadFileJob : Job
{
    public required string FilePath { get; init; }

    public override string Group => "LOAD";

    public byte[]? Contents { get; set; }
}

public class LoadFileJobHandler : IJobHandler<LoadFileJob>
{
    public async Task HandleAsync(LoadFileJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("executing: " + job.Id);

        await Task.Delay(10000, cancellationToken);
        job.Contents = await File.ReadAllBytesAsync(job.FilePath, cancellationToken);
    }
}