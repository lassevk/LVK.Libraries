namespace LasseVK.Events;

public interface IEvents
{
    Task PublishAsync<T>(T evt, CancellationToken token = default);

    IDisposable Subscribe<T>(Action<T> subscriber);
    IDisposable Subscribe<T>(IEventSubscriber<T> subscriber);
}