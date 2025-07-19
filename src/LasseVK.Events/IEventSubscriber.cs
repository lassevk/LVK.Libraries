namespace LasseVK.Events;

public interface IEventSubscriber<in T> : IEventSubscriber
{
    Task HandleAsync(T evt, CancellationToken token);
}

public interface IEventSubscriber;