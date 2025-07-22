namespace LVK.Bootstrapping;

public interface IModuleInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}