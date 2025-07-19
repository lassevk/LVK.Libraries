using System.Collections.Concurrent;

using Microsoft.Extensions.DependencyInjection;

namespace LVK.Events;

internal class Events : IEvents
{
    private readonly IServiceProvider _serviceProvider;

    private readonly ConcurrentDictionary<Type, ConcurrentDictionary<object, bool>> _subscribers = new();

    public Events(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public async Task PublishAsync<T>(T evt, CancellationToken token = default)
    {
        IEnumerable<IEventSubscriber<T>> subscribers = _serviceProvider.GetServices<IEventSubscriber<T>>();
        _subscribers.TryGetValue(typeof(T), out ConcurrentDictionary<object, bool>? subscriberObjects);
        if (subscriberObjects != null)
        {
            subscribers = subscribers.Concat(subscriberObjects.Keys.OfType<IEventSubscriber<T>>().ToList());
        }

        await Task.WhenAll(subscribers.Select(subscriber => subscriber.HandleAsync(evt, token)));
    }

    public IDisposable Subscribe<T>(IEventSubscriber<T> subscriber) => Subscribe(subscriber, typeof(T));

    private IDisposable Subscribe(object subscriber, Type subscriptionType)
    {
        ConcurrentDictionary<object, bool> subscriberList = _subscribers.GetOrAdd(subscriptionType, _ => []);
        subscriberList.AddOrUpdate(subscriber, true, (_, _) => true);

        return Disposable.Create(() =>
        {
            subscriberList.Remove(subscriber, out _);
        });
    }

    public IDisposable AutoSubscribe(IEventSubscriber subscriber)
    {
        var subscriptions = new List<IDisposable>();

        ArgumentNullException.ThrowIfNull(subscriber);

        foreach (Type interfaceType in subscriber.GetType().GetInterfaces())
        {
            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IEventSubscriber<>))
            {
                subscriptions.Add(Subscribe(subscriber, interfaceType.GenericTypeArguments[0]));
            }
        }

        return Disposable.Create(subscriptions);
    }
}