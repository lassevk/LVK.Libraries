namespace LasseVK.Events.Tests;

public class AutoSubscriber : IEventSubscriber<int>, IEventSubscriber<string>
{
    public Task HandleAsync(int evt, CancellationToken token)
    {
        IntMessage = evt;
        return Task.CompletedTask;
    }

    public Task HandleAsync(string evt, CancellationToken token)
    {
        StringMessage = evt;
        return Task.CompletedTask;
    }

    public int IntMessage { get; private set; }
    public string StringMessage { get; private set; } = "";
}