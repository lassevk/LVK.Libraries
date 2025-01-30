using LasseVK.Bootstrapping.ConsoleApplications;

namespace Sandbox.Console;

public class Application : IConsoleApplication
{
    public Task<int> RunAsync(CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Test");
        return Task.FromResult(0);
    }
}