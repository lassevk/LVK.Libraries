using LasseVK.Bootstrapping.ConsoleApplications;
using LasseVK.Jobs;

using Microsoft.Extensions.Configuration;

using Sandbox.Console.Jobs;

namespace Sandbox.Console;

public class Application : IConsoleApplication
{
    private readonly IConfiguration _configuration;

    public Application(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        System.Console.WriteLine(_configuration["Config"]);

        return 0;
    }
}