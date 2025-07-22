using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Npgsql;

namespace LVK.Data.PostgreSql;

internal class PostgreSqlNotificationListener<T> : IPostgreSqlNotificationListener<T>
    where T : DbContext
{
    private readonly IDbContextFactory<T> _dbContextFactory;
    private readonly ILogger<PostgreSqlNotificationListener<T>> _logger;
    private readonly Dictionary<string, List<Action<string>>> _listeners = new();

    public PostgreSqlNotificationListener(IDbContextFactory<T> dbContextFactory, ILogger<PostgreSqlNotificationListener<T>> logger)
    {
        _dbContextFactory = dbContextFactory ?? throw new ArgumentNullException(nameof(dbContextFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void AddListener(string channel, Action<string> callback)
    {
        if (!_listeners.TryGetValue(channel, out List<Action<string>>? listeners))
        {
            listeners = new List<Action<string>>();
            _listeners.Add(channel, listeners);
        }

        listeners.Add(callback);
    }

    public async Task ListenAsync(CancellationToken cancellationToken)
    {
        await using T dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await using var connection = (NpgsqlConnection)dbContext.Database.GetDbConnection();
        try
        {
            await connection.OpenAsync(cancellationToken);

            connection.Notification += (_, args) =>
            {
                _logger.LogDebug("Received notification on channel {Channel}: {Payload}", args.Channel, args.Payload);
                if (!_listeners.TryGetValue(args.Channel, out List<Action<string>>? listeners))
                {
                    return;
                }

                foreach (Action<string> action in listeners)
                {
                    action(args.Payload);
                }
            };

            foreach (string channel in _listeners.Keys)
            {
                _logger.LogDebug("Listening on channel {Channel}", channel);
                await connection.ExecuteAsync("LISTEN " + channel, cancellationToken: cancellationToken);
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                await connection.WaitAsync(cancellationToken);
            }
        }
        catch (TaskCanceledException)
        {
            // Ignore this
        }
        finally
        {
            _logger.LogDebug("Stopped listening for notifications");
        }
    }
}