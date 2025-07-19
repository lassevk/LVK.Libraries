using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LVK.Bootstrapping.ConsoleApplications;

internal class ConsoleApplicationHostedService : BackgroundService
{
    private readonly IConsoleApplication _consoleApplication;
    private readonly IHostApplicationLifetime _hostApplicationLifetime;
    private readonly ILogger<ConsoleApplicationHostedService> _logger;

    public ConsoleApplicationHostedService(IConsoleApplication consoleApplication, IHostApplicationLifetime hostApplicationLifetime, ILogger<ConsoleApplicationHostedService> logger)
    {
        _consoleApplication = consoleApplication ?? throw new ArgumentNullException(nameof(consoleApplication));
        _hostApplicationLifetime = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            var cts = new CancellationTokenSource();
            _hostApplicationLifetime.ApplicationStopping.Register(() => cts.Cancel());
            stoppingToken.Register(() => cts.Cancel());

            _logger.LogDebug("Starting console application");
            Environment.ExitCode = await _consoleApplication.RunAsync(cts.Token);
            _logger.LogDebug("Console application stopped, exit code = {ExitCode}", Environment.ExitCode);
        }
        catch (TaskCanceledException)
        {
            _logger.LogDebug("Console application canceled");
            if (Environment.ExitCode == 0)
            {
                Environment.ExitCode = 1;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Console application failed");
            Console.WriteLine(ex);
            Environment.ExitCode = 2;
        }
        finally
        {
            _hostApplicationLifetime.StopApplication();
        }
    }
}