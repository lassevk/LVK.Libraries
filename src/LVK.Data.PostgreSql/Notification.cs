namespace LVK.Data.PostgreSql;

internal readonly record struct Notification(string Channel, string Payload);