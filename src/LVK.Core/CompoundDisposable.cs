namespace LVK;

internal sealed class CompoundDisposable : IDisposable
{
    private IDisposable[]? _disposables;

    public CompoundDisposable(IDisposable[] disposables)
    {
        _disposables = disposables ?? throw new ArgumentNullException(nameof(disposables));
    }

    public void Dispose()
    {
        IDisposable[]? disposables = Interlocked.Exchange(ref _disposables, null);
        if (disposables == null)
        {
            return;
        }

        foreach (IDisposable disposable in disposables)
        {
            disposable?.Dispose();
        }
    }
}