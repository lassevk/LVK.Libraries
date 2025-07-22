namespace LVK.EntityFramework.PostgreSQL;

internal class NotificationsCollection
{
    private List<Notification> _notifications = [];

    public void AddNotification(string channel, string payload)
    {
        _notifications.Add(new Notification(channel, payload));
    }

    public List<Notification> GetNotifications() => Interlocked.Exchange(ref _notifications, []);
}