namespace LVK.Bootstrapping.Maui;

public interface IMauiAppInitializer
{
    Task InitializeAsync(MauiApp host);
}