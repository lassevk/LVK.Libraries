namespace LasseVK;

internal sealed class ActionDisposable : IDisposable
{
    private Action? _action;

    public ActionDisposable(Action action)
    {
        _action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public void Dispose()
    {
        Interlocked.Exchange(ref _action, null)?.Invoke();
    }
}