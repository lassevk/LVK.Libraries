using System.ComponentModel;

using JetBrains.Annotations;

namespace LVK;

[EditorBrowsable(EditorBrowsableState.Never)]
[PublicAPI]
public static class CancellationTokenExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static CancellationTokenAwaiter GetAwaiter(this CancellationToken cancellationToken) => new(cancellationToken, true);

    public static CancellationTokenAwaiter AwaitThrowsTaskCancelledException(this CancellationToken cancellationToken) => new(cancellationToken, false);
}