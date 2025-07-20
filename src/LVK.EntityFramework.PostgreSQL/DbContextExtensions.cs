using System.Text.Json;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace LVK.EntityFramework.PostgreSQL;

public static class DbContextExtensions
{
    public static DbContext Notify<T>(this DbContext dbContext, string channel, T payload)
    {
        NotificationsCollection notifications = dbContext.GetService<NotificationsCollection>();
        string json = JsonSerializer.Serialize(payload);
        notifications.Notify(channel, json);

        return dbContext;
    }
}