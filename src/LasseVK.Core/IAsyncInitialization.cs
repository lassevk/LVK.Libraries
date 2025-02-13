namespace LasseVK;

public interface IAsyncInitialization
{
    Task WaitForInitializationAsync();
}