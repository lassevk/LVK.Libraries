using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class LoadFileJobHandler : IJobHandler<LoadFileJob>
{
    public async Task HandleAsync(LoadFileJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("executing: " + job.Id);

        if (Random.Shared.Next(0, 100) > 50)
        {
            throw new InvalidOperationException("Random failure");
        }

        job.Contents = await File.ReadAllBytesAsync(job.FilePath, cancellationToken);
    }
}