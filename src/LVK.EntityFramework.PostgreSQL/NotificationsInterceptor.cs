using System.Data.Common;

using Dapper;

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;

namespace LVK.EntityFramework.PostgreSQL;

internal class NotificationsInterceptor : DbConnectionInterceptor
{
    private readonly NotificationsCollection _notificationsCollection;
    private readonly ILogger<NotificationsInterceptor> _logger;

    public NotificationsInterceptor(NotificationsCollection notificationsCollection, ILogger<NotificationsInterceptor> logger)
    {
        _notificationsCollection = notificationsCollection ?? throw new ArgumentNullException(nameof(notificationsCollection));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        base.ConnectionOpened(connection, eventData);
        foreach (CommandDefinition command in GetNotificationCommands(CancellationToken.None))
        {
            _logger.LogDebug("Sending notification {Parameters}", command.Parameters);
            connection.Execute(command);
        }
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new())
    {
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        foreach (CommandDefinition command in GetNotificationCommands(cancellationToken))
        {
            _logger.LogDebug("Sending notification {Parameters}", command.Parameters);
            await connection.ExecuteAsync(command);
        }
    }

    private IEnumerable<CommandDefinition> GetNotificationCommands(CancellationToken cancellationToken) => _notificationsCollection.GetNotifications().Select(notification => new CommandDefinition("SELECT pg_notify (@channel, @payload)", new
    {
        channel = notification.Channel, payload = notification.Payload,
    }, cancellationToken: cancellationToken));
}