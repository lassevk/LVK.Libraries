using JetBrains.Annotations;

namespace LasseVK.Bootstrapping.ConsoleApplications;

public interface IConsoleApplication
{
    Task<int> RunAsync(CancellationToken cancellationToken);
}