namespace LVK.Events;

internal class ActionSubscriber<T> : IEventSubscriber<T>
{
    private readonly Action<T> _subscriber;

    public ActionSubscriber(Action<T> subscriber)
    {
        _subscriber = subscriber ?? throw new ArgumentNullException(nameof(subscriber));
    }

    public async Task HandleAsync(T evt, CancellationToken token)
    {
        await Task.Yield();
        _subscriber(evt);
    }
}