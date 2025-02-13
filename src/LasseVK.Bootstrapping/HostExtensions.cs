using JetBrains.Annotations;

using LasseVK.Bootstrapping.ConsoleApplications;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LasseVK.Bootstrapping;

[PublicAPI]
public static class HostExtensions
{

    public static async Task RunAsyncEx<THost>(this THost host, CancellationToken cancellationToken = default)
        where THost : IHost
    {
        try
        {
            IEnumerable<IHostInitializer<THost>> initializers = host.Services.GetServices<IHostInitializer<THost>>();
            foreach (IHostInitializer<THost> initializer in initializers)
            {
                await initializer.InitializeAsync(host, cancellationToken);
            }

            await host.StartAsync(cancellationToken);
            foreach (IAsyncInitialization initialization in host.Services.GetServices<IAsyncInitialization>())
            {
                await initialization.WaitForInitializationAsync();
            }

            IConsoleApplication? consoleApplication = host.Services.GetService<IConsoleApplication>();
            if (consoleApplication != null)
            {
                IHostApplicationLifetime hal = host.Services.GetRequiredService<IHostApplicationLifetime>();

                var cts = new CancellationTokenSource();
                hal.ApplicationStopping.Register(() => cts.Cancel());

                try
                {
                    await RunConsoleApplicationAsync(host, consoleApplication, cts.Token);
                }
                finally
                {
                    await host.StopAsync(cancellationToken);
                }
            }
            else
            {
                await host.WaitForShutdownAsync(cancellationToken);
            }
        }
        finally
        {
            host.Dispose();
        }
    }

    private static async Task RunConsoleApplicationAsync<THost>(THost host, IConsoleApplication consoleApplication, CancellationToken cancellationToken)
        where THost : IHost
    {
        try
        {
            Environment.ExitCode = await consoleApplication.RunAsync(cancellationToken);
        }
        catch (TaskCanceledException)
        {
            if (Environment.ExitCode == 0)
            {
                Environment.ExitCode = 1;
            }

            throw;
        }
        catch (Exception ex)
        {
            if (Environment.ExitCode == 0)
            {
                Environment.ExitCode = 2;
            }

            host.Services.GetRequiredService<ILogger>().LogError(ex, ex.Message);
            throw;
        }
        finally
        {
            // ReSharper disable once SuspiciousTypeConversion.Global
            (consoleApplication as IDisposable)?.Dispose();
        }
    }
}