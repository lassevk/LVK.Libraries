namespace LasseVK;

public sealed class AsyncActionDisposable : IAsyncDisposable
{
    private Func<Task>? _action;

    public AsyncActionDisposable(Func<Task> action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public async ValueTask DisposeAsync()
    {
        Task? task = Interlocked.Exchange(ref _action, null)?.Invoke();
        if (task != null)
        {
            await task.ConfigureAwait(false);
        }
    }
}