using System.Text.Json;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace LVK.EntityFramework.PostgreSQL;

[PublicAPI]
public static class DbContextExtensions
{
    public static void AddNotification(this DbContext dbContext, string channel, string payload)
    {
        NotificationsCollection notificationsCollection = dbContext.GetService<NotificationsCollection>();
        notificationsCollection.AddNotification(channel, payload);

        ILogger<DbContext> logger = dbContext.GetService<ILogger<DbContext>>();
        logger.LogDebug("Added notification on channel {Channel}: {Payload}", channel, payload);
    }

    public static void AddNotification<T>(this DbContext dbContext, string channel, T payload)
    {
        string json = JsonSerializer.Serialize(payload);
        AddNotification(dbContext, channel, json);
    }

    public static void SendNotifications(this DbContext dbContext)
    {
        dbContext.Database.OpenConnection();
        dbContext.Database.CloseConnection();
    }

    public static async Task SendNotificationsAsync(this DbContext dbContext, CancellationToken cancellationToken = default)
    {
        await dbContext.Database.OpenConnectionAsync(cancellationToken);
        await dbContext.Database.CloseConnectionAsync();
    }
}