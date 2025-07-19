namespace LVK.Pushover;

public interface IPushover
{
    Task SendAsync(string message, CancellationToken cancellationToken = default)
        => SendAsync(new PushoverNotification(message), cancellationToken);

    Task SendAsync(PushoverNotification notification, CancellationToken cancellationToken = default);
}