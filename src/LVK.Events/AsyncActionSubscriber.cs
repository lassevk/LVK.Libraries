namespace LVK.Events;

internal class AsyncActionSubscriber<T> : IEventSubscriber<T>
{
    private readonly Func<T, Task> _subscriber;

    public AsyncActionSubscriber(Func<T, Task> subscriber)
    {
        _subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
    }

    public async Task HandleAsync(T evt, CancellationToken token) => await _subscriber(evt);
}