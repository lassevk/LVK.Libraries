using System.Text.Json;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace LVK.Data.PostgreSql;

[PublicAPI]
public static class DbContextExtensions
{
    extension(DbContext dbContext)
    {
        public void AddNotification(string channel, string payload)
        {
            NotificationsCollection notificationsCollection = dbContext.GetService<NotificationsCollection>();
            notificationsCollection.AddNotification(channel, payload);

            ILogger<DbContext> logger = dbContext.GetService<ILogger<DbContext>>();
            logger.LogDebug("Added notification on channel {Channel}: {Payload}", channel, payload);
        }

        public void AddNotification<T>(string channel, T payload)
        {
            string json = JsonSerializer.Serialize(payload);
            AddNotification(dbContext, channel, json);
        }

        public void SendNotifications()
        {
            dbContext.Database.OpenConnection();
            dbContext.Database.CloseConnection();
        }

        public async Task SendNotificationsAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.Database.OpenConnectionAsync(cancellationToken);
            await dbContext.Database.CloseConnectionAsync();
        }
    }
}