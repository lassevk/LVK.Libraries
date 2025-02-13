namespace LasseVK.Ssh;

internal class SshAsyncInitialization : IAsyncInitialization
{
    private readonly TaskCompletionSource _tcs = new();

    public void SetInitialized() => _tcs.SetResult();

    public async Task WaitForInitializationAsync()
    {
        await _tcs.Task;
    }
}