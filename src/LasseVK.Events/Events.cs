using System.Collections.Concurrent;

using Microsoft.Extensions.DependencyInjection;

namespace LasseVK.Events;

internal class Events : IEvents
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<Type, List<object>> _subscribers = new();

    public Events(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task PublishAsync<T>(T evt, CancellationToken token = default)
    {
        IEnumerable<IEventSubscriber<T>> subscribers = _serviceProvider.GetServices<IEventSubscriber<T>>();
        _subscribers.TryGetValue(typeof(T), out List<object>? subscriberObjects);
        if (subscriberObjects != null)
        {
            lock (subscriberObjects)
            {
                subscribers = subscribers.Concat(subscriberObjects.OfType<IEventSubscriber<T>>().ToList());
            }
        }

        await Task.WhenAll(subscribers.Select(subscriber => subscriber.HandleAsync(evt, token)));
    }

    public IDisposable Subscribe<T>(IEventSubscriber<T> subscriber)
    {
        List<object> subscriberList = _subscribers.GetOrAdd(typeof(T), _ => new List<object>());
        lock (subscriberList)
        {
            subscriberList.Add(subscriber);
        }

        return new ActionDisposable(() =>
        {
            lock (subscriberList)
            {
                subscriberList.Remove(subscriber);
            }
        });
    }
}