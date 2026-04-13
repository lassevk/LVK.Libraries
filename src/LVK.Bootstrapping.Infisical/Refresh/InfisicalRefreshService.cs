using Humanizer;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LVK.Bootstrapping.Infisical.Refresh;

internal class InfisicalRefreshService : BackgroundService
{
    private readonly IOptions<InfisicalRefreshServiceOptions> _options;
    private readonly ILogger<InfisicalRefreshService> _logger;
    private readonly InfisicalRefreshChannel? _refreshChannel;

    public InfisicalRefreshService(IOptions<InfisicalRefreshServiceOptions> options,
            ILogger<InfisicalRefreshService> logger,
            InfisicalRefreshChannel? refreshChannel = null)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _refreshChannel = refreshChannel;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_refreshChannel is null)
        {
            return;
        }

        Secret[] secrets = SortedSecrets(_options.Value.Secrets);
        var comparer = new SecretComparer();
        while (!stoppingToken.IsCancellationRequested)
        {

            _logger.LogInformation("Waiting for {Delay} before refreshing Infisical secrets", _options.Value.RefreshIntervalSeconds.Seconds());
            await Task.Delay(TimeSpan.FromSeconds(_options.Value.RefreshIntervalSeconds), stoppingToken);
            if (_refreshChannel.Provider is null)
            {
                continue;
            }

            _logger.LogInformation("Refreshing Infisical secrets");

            Secret[] newSecrets = SortedSecrets(await _options.Value.Service.GetSecretsAsync());

            if (secrets.SequenceEqual(newSecrets, comparer))
            {
                continue;
            }

            _logger.LogInformation("Triggering configuration change due to updated secrets");
            secrets = newSecrets;
            _refreshChannel.Provider.Update(newSecrets);
        }
    }

    private static Secret[] SortedSecrets(Secret[] secrets) => secrets.OrderBy(s => s.SecretKey).ToArray();
}