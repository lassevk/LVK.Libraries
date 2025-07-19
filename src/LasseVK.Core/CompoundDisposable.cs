namespace LasseVK;

internal sealed class CompoundDisposable : IDisposable
{
    private readonly IDisposable[] _disposables;

    public CompoundDisposable(IDisposable[] disposables)
    {
        _disposables = disposables ?? throw new ArgumentNullException(nameof(disposables));
    }

    public void Dispose()
    {
        foreach (IDisposable disposable in _disposables)
        {
            disposable.Dispose();
        }
    }
}