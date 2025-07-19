namespace LasseVK;

public static class Disposable
{
    public static IDisposable Create(Action action) => new ActionDisposable(action);

    public static IDisposable Create(params IEnumerable<IDisposable> disposables) => new CompoundDisposable(disposables.ToArray());
}