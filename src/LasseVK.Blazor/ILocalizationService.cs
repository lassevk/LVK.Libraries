namespace LasseVK.Blazor;

public interface ILocalizationService
{
    string this[string key] { get; }

    Task SetLanguageAsync(string language, bool reload = true);
}