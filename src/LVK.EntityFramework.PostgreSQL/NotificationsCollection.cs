namespace LVK.EntityFramework.PostgreSQL;

internal class NotificationsCollection
{
    private List<(string channel, string payload)> _notifications = [];

    public void Notify(string channel, string payload)
    {
        _notifications.Add((channel, payload));
    }

    public List<(string channel, string payload)> GetNotifications() => Interlocked.Exchange(ref _notifications, []);
}