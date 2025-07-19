using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

namespace LVK.Events.Tests;

public class EventsTests
{
    [Test]
    public async Task Publish_NotSubscribed_DoesNotTriggerSubscriber()
    {
        var serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var events = new Events(serviceProvider);

        await events.PublishAsync("TEST");
    }

    [Test]
    public async Task Publish_EventWithActionSubscriber_CallsAction()
    {
        var serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        IEvents events = new Events(serviceProvider);

        bool called = false;
        using IDisposable subscription = events.Subscribe((string _) =>
        {
            called = true;
        });

        await events.PublishAsync("TEST");

        Assert.That(called, Is.True);
    }

    [Test]
    public async Task Publish_EventWithAsyncActionSubscriber_CallsAction()
    {
        var serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        IEvents events = new Events(serviceProvider);

        bool called = false;
        using IDisposable subscription = events.Subscribe((string _) =>
        {
            called = true;
            return Task.CompletedTask;
        });

        await events.PublishAsync("TEST");

        Assert.That(called, Is.True);
    }

    [Test]
    public async Task Publish_EventWithEventSubscriber_CallsAction()
    {
        var serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var events = new Events(serviceProvider);

        IEventSubscriber<string>? subscriber = Substitute.For<IEventSubscriber<string>>();

        using IDisposable subscription = events.Subscribe(subscriber);

        await events.PublishAsync("TEST");

        _ = subscriber.Received().HandleAsync("TEST", Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Publish_EventWithUnsubscribedEventSubscriber_DoesNotCallAction()
    {
        var serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var events = new Events(serviceProvider);

        IEventSubscriber<string>? subscriber = Substitute.For<IEventSubscriber<string>>();

        using (events.Subscribe(subscriber))
        {
        }

        await events.PublishAsync("TEST");

        _ = subscriber.DidNotReceive().HandleAsync("TEST", Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Publish_EventRegisteredAsService_CallsEventHandler()
    {
        IEventSubscriber<string>? handler = Substitute.For<IEventSubscriber<string>>();
        handler.HandleAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton(handler);
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        var events = new Events(serviceProvider);

        await events.PublishAsync("TEST");

        _ = handler.Received().HandleAsync("TEST", Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task AutoSubscribe_SubscribesToAllEvents()
    {
        var subscriber = new AutoSubscriber();

        var serviceCollection = new ServiceCollection();
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        var events = new Events(serviceProvider);

        events.AutoSubscribe(subscriber);
        await events.PublishAsync(1);
        await events.PublishAsync("TEST");

        Assert.That(subscriber.IntMessage, Is.EqualTo(1));
        Assert.That(subscriber.StringMessage, Is.EqualTo("TEST"));
    }
}