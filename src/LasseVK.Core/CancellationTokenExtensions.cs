namespace LasseVK;

public static class CancellationTokenExtensions
{
    public static CancellationTokenAwaitable GetAwaiter(this CancellationToken cancellationToken) => new CancellationTokenAwaitable(cancellationToken);
}