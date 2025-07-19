namespace LVK.Hosting.ConsoleApplications;

public interface IConsoleApplication
{
    Task<int> RunAsync(CancellationToken cancellationToken);
}
