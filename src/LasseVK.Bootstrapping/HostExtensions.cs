using LasseVK.Bootstrapping.ConsoleApplications;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LasseVK.Bootstrapping;

public static class HostExtensions
{
    public static async Task RunAsConsoleApplicationAsync<T>(this IHost host, CancellationToken cancellationToken = default)
        where T : class, IConsoleApplication
    {
        IHostApplicationLifetime hal = host.Services.GetRequiredService<IHostApplicationLifetime>();

        var cts = new CancellationTokenSource();
        hal.ApplicationStopping.Register(() => cts.Cancel());

        T consoleApplication = ActivatorUtilities.CreateInstance<T>(host.Services);
        try
        {
            int exitCode = await consoleApplication.RunAsync(cts.Token);
            Environment.ExitCode = exitCode;
        }
        catch (TaskCanceledException)
        {
            if (Environment.ExitCode != 0)
            {
                Environment.ExitCode = 1;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Environment.ExitCode = 2;
        }
    }

    public static async Task InitializeAsync<T>(this T host, CancellationToken cancellationToken = default)
        where T : IHost
    {
        IEnumerable<IHostInitializer<T>> initializers = host.Services.GetServices<IHostInitializer<T>>();
        foreach (IHostInitializer<T> initializer in initializers)
        {
            await initializer.InitializeAsync(host, cancellationToken);
        }
    }
}