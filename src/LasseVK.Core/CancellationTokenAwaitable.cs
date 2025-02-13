using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LasseVK;

[EditorBrowsable(EditorBrowsableState.Never)]
public class CancellationTokenAwaitable : INotifyCompletion, ICriticalNotifyCompletion
{
    private readonly CancellationToken _cancellationToken;

    public CancellationTokenAwaitable(CancellationToken cancellationToken)
    {
        _cancellationToken = cancellationToken;
    }

    public object GetResult()
    {
        if (IsCompleted)
        {
            throw new OperationCanceledException();
        }

        throw new InvalidOperationException("The cancellation token has not yet been completed.");
    }

    public bool IsCompleted => _cancellationToken.IsCancellationRequested;

    public void OnCompleted(Action continuation) => _cancellationToken.Register(continuation);
    public void UnsafeOnCompleted(Action continuation) => _cancellationToken.Register(continuation);
}