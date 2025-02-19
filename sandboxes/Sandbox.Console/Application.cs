using LasseVK.Bootstrapping.ConsoleApplications;
using LasseVK.Pushover;

using Microsoft.Extensions.Configuration;

namespace Sandbox.Console;

public class Application : IConsoleApplication
{
    private readonly IConfiguration _configuration;
    private readonly IPushover _pushover;

    public Application(IConfiguration configuration, IPushover pushover)
    {
        _configuration = configuration;
        _pushover = pushover;
    }
    public async Task<int> RunAsync(CancellationToken cancellationToken)
    {
        await Task.Yield();
        System.Console.WriteLine(_configuration["Config"]);
        // await _pushover.SendAsync("Test", cancellationToken);

        return 0;
    }
}