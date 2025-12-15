using System.ComponentModel;

using JetBrains.Annotations;

namespace LVK;

[EditorBrowsable(EditorBrowsableState.Never)]
[PublicAPI]
public static class CancellationTokenExtensions
{
    extension(CancellationToken cancellationToken)
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CancellationTokenAwaiter GetAwaiter() => new(cancellationToken, true);

        public CancellationTokenAwaiter AwaitThrowsTaskCancelledException() => new(cancellationToken, false);
    }
}