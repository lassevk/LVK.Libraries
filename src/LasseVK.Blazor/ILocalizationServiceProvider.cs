namespace LasseVK.Blazor;

internal interface ILocalizationServiceProvider
{
    ILocalizationService GetService(Type type);
}