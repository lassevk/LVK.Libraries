namespace LVK.EntityFramework.PostgreSQL;

internal readonly record struct Notification(string Channel, string Payload);