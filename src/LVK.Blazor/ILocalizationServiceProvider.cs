namespace LVK.Blazor;

internal interface ILocalizationServiceProvider
{
    ILocalizationService GetService(Type type);
}