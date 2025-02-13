using LasseVK.Bootstrapping.ConsoleApplications;

namespace Sandbox.Console;

public class Application : IConsoleApplication
{
    public async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();

        System.Console.WriteLine("Press enter to close proxy");
        System.Console.ReadLine();

        return 0;
    }
}