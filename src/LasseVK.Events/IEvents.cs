namespace LasseVK.Events;

public interface IEvents
{
    Task PublishAsync<T>(T evt, CancellationToken token = default);

    IDisposable Subscribe<T>(Action<T> subscriber) => Subscribe(new ActionSubscriber<T>(subscriber));
    IDisposable Subscribe<T>(Func<T, Task> subscriber) => Subscribe(new AsyncActionSubscriber<T>(subscriber));

    IDisposable Subscribe<T>(IEventSubscriber<T> subscriber);

    IDisposable AutoSubscribe(IEventSubscriber subscriber);
}