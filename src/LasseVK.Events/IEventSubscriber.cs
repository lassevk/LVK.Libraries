namespace LasseVK.Events;

public interface IEventSubscriber<in T>
{
    Task HandleAsync(T evt, CancellationToken token);
}