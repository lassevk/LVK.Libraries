using System.Text;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Renci.SshNet;

using static LasseVK.Assertions;

namespace LasseVK.Ssh;

internal class SshProxyService : BackgroundService
{
    private readonly ILogger<SshProxyService> _logger;
    private readonly IOptions<SshProxyOptions> _options;
    private readonly SshAsyncInitialization _initialization;

    private readonly List<SshClient> _sshClients = new();
    private readonly List<ForwardedPortLocal> _ports = new();

    public SshProxyService(ILogger<SshProxyService> logger, IOptions<SshProxyOptions> options, SshAsyncInitialization initialization)
    {
        _logger = logger;
        _options = options;
        _initialization = initialization;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            await StartProxyAsync(stoppingToken);
            _initialization.SetInitialized();
            await stoppingToken;
        }
        finally
        {
            StopProxy();
        }
    }

    private async Task StartProxyAsync(CancellationToken cancellationToken)
    {
        if ((_options.Value.Ports?.Count ?? 0) == 0)
        {
            return;
        }

        assume(_options.Value.Ports != null);

        SshClient? commonClient = null;
        if (_options.Value.Host != null)
        {
            commonClient = await ConnectClientAsync(_options.Value.Host, cancellationToken);
            _sshClients.Add(commonClient);
        }

        foreach (SshProxyPort port in _options.Value.Ports)
        {
            SshClient? client = commonClient;
            if (port.Host != null)
            {
                client = await ConnectClientAsync(port.Host, cancellationToken);
                _sshClients.Add(client);
            }
            assume(client != null);

            var forwardedPort = new ForwardedPortLocal("localhost", (uint)port.LocalPort, port.RemoteHost ?? "127.0.0.1", (uint)(port.RemotePort ?? port.LocalPort));

            _logger.LogInformation("Opening local port {LocalPort} forwarded to remote {RemoteHost}:{RemotePort}", port.LocalPort, port.RemoteHost ?? "127.0.0.1", port.RemotePort ?? port.LocalPort);
            client.AddForwardedPort(forwardedPort);
            forwardedPort.Start();
            _ports.Add(forwardedPort);
        }
    }

    private async Task<SshClient> ConnectClientAsync(SshProxyHost host, CancellationToken cancellationToken)
    {
        SshClient client = CreateClient(host);
        _logger.LogInformation("Connecting to {User}@{Host}:{Port}", host.Authentication!.Username, host.Hostname, host.Port);
        await client.ConnectAsync(cancellationToken);
        _logger.LogInformation("Connected to {User}@{Host}:{Port}", host.Authentication!.Username, host.Hostname, host.Port);
        return client;
    }

    private static SshClient CreateClient(SshProxyHost host)
    {
        assume(host.Authentication != null);

        if (host.Authentication.Password != null)
        {
            return new SshClient(host.Hostname, host.Port, host.Authentication.Password);
        }

        if (host.Authentication.CertificatePath != null)
        {
            return new SshClient(host.Hostname, host.Port, host.Authentication.Username, new PrivateKeyFile(host.Authentication.CertificatePath, host.Authentication.Password));
        }

        assume(host.Authentication.Certificate != null);
        var stream = new MemoryStream(Convert.FromBase64String(host.Authentication.Certificate));
        return new SshClient(host.Hostname, host.Port, host.Authentication.Username, new PrivateKeyFile(stream, host.Authentication.Password));
    }

    private void StopProxy()
    {
        foreach (ForwardedPortLocal port in _ports)
        {
            port.Stop();
        }

        _ports.Clear();

        foreach (SshClient client in _sshClients)
        {
            _logger.LogInformation("Disconnecting client for {Username}@{Hostname}:{Port}", client.ConnectionInfo.Username, client.ConnectionInfo.Host, client.ConnectionInfo.Port);
            client.Disconnect();
        }

        _sshClients.Clear();
    }

    public override void Dispose()
    {
        base.Dispose();
        foreach (ForwardedPortLocal port in _ports)
        {
            port.Dispose();
        }
        foreach (SshClient client in _sshClients)
        {
            client.Dispose();
        }
    }
}