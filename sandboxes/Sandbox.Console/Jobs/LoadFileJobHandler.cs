using LasseVK.Jobs;

namespace Sandbox.Console.Jobs;

public class LoadFileJobHandler : IJobHandler<LoadFileJob>
{
    private readonly IJobLogger _logger;

    public LoadFileJobHandler(IJobLogger logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task HandleAsync(LoadFileJob job, CancellationToken cancellationToken)
    {
        System.Console.WriteLine("executing: " + job.Id);

        // if (Random.Shared.Next(0, 100) > 50)
        // {
        //     throw new InvalidOperationException("Random failure");
        // }

        job.Contents = await File.ReadAllBytesAsync(job.FilePath, cancellationToken);
        await _logger.AddLogAsync($"Loaded {job.Contents.Length} bytes from {job.FilePath}", cancellationToken);
    }
}