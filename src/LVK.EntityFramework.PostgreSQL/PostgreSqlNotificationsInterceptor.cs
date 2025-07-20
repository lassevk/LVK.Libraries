using System.Data.Common;

using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LVK.EntityFramework.PostgreSQL;

internal class PostgreSqlNotificationsInterceptor : DbConnectionInterceptor
{
    private readonly NotificationsCollection _notificationsCollection;

    public PostgreSqlNotificationsInterceptor(NotificationsCollection notificationsCollection)
    {
        _notificationsCollection = notificationsCollection ?? throw new ArgumentNullException(nameof(notificationsCollection));
    }

    public override void ConnectionOpened(DbConnection connection, ConnectionEndEventData eventData)
    {
        base.ConnectionOpened(connection, eventData);
        SendNotifications(connection);
    }

    public override async Task ConnectionOpenedAsync(DbConnection connection, ConnectionEndEventData eventData, CancellationToken cancellationToken = new CancellationToken())
    {
        await base.ConnectionOpenedAsync(connection, eventData, cancellationToken);
        SendNotifications(connection);
    }

    private void SendNotifications(DbConnection connection)
    {
        List<(string channel, string payload)> notifications = _notificationsCollection.GetNotifications();
        foreach ((string channel, string payload) notification in notifications)
        {
            DbCommand cmd = connection.CreateCommand();

            string fixedPayload = notification.payload.Replace("'", "''");
            Console.WriteLine($"sending notification: {notification.channel}, {fixedPayload}");
            cmd.CommandText = $"NOTIFY {notification.channel}, '{fixedPayload}'";
            cmd.ExecuteNonQuery();
        }
    }
}