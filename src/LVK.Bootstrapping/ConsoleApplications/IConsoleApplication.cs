namespace LVK.Bootstrapping.ConsoleApplications;

public interface IConsoleApplication
{
    Task<int> RunAsync(CancellationToken cancellationToken);
}