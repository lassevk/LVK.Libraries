using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

[JobIdentifier("load-file")]
public class LoadFileJob : Job
{
    public required string FilePath { get; init; }

    public override string Group => "LOAD";

    public byte[]? Contents { get; set; }

    public override string ToString() => $"{base.ToString()} {FilePath}";
}

public class LoadFileJobHandler : IJobHandler<LoadFileJob>
{
    public async Task HandleAsync(LoadFileJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("executing: " + job.Id);

        job.Contents = await File.ReadAllBytesAsync(job.FilePath, cancellationToken);
    }
}