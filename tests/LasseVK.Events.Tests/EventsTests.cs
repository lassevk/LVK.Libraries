using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace LasseVK.Events.Tests;

public class EventsTests : IDisposable
{
    private readonly ServiceProvider _serviceProvider;
    private readonly Events _events;

    public EventsTests()
    {
        var serviceCollection = new ServiceCollection();
        _serviceProvider = serviceCollection.BuildServiceProvider();

        _events = new Events(_serviceProvider);
    }

    public void Dispose()
    {
        _serviceProvider.Dispose();
    }

    [Test]
    public async Task Publish_NotSubscribed_DoesNotTriggerSubscriber()
    {
        await _events.PublishAsync("TEST");
    }

    [Test]
    public async Task Publish_EventWithActionSubscriber_CallsAction()
    {
        bool called = false;
        using IDisposable subscription = _events.Subscribe((string s) =>
        {
            called = true;
        });

        await _events.PublishAsync("TEST");

        Assert.That(called, Is.True);
    }

    [Test]
    public async Task Publish_EventWithEventSubscriber_CallsAction()
    {
        IEventSubscriber<string>? subscriber = Substitute.For<IEventSubscriber<string>>();

        using IDisposable subscription = _events.Subscribe(subscriber);

        await _events.PublishAsync("TEST");

        _ = subscriber.Received().HandleAsync("TEST", Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Publish_EventWithUnsubscribedEventSubscriber_DoesNotCallAction()
    {
        IEventSubscriber<string>? subscriber = Substitute.For<IEventSubscriber<string>>();

        using (_events.Subscribe(subscriber))
        {
        }

        await _events.PublishAsync("TEST");

        _ = subscriber.DidNotReceive().HandleAsync("TEST", Arg.Any<CancellationToken>());
    }
}