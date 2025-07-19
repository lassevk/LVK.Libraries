using System.Resources;

using Microsoft.JSInterop;

namespace LVK.Blazor;

internal class LocalizationService : ILocalizationService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly GlobalResourceProvider? _globalResourceProvider;
    private readonly ResourceManager _resourceManager;

    public LocalizationService(IJSRuntime jsRuntime, GlobalResourceProvider? globalResourceProvider, ResourceManager resourceManager)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _globalResourceProvider = globalResourceProvider;
        _resourceManager = resourceManager;
    }

    public string this[string key]
    {
        get
        {
            try
            {
                return _resourceManager.GetString(key) ?? _globalResourceProvider?.GetString(key) ?? $"{{{key}}}";
            }
            catch (MissingManifestResourceException)
            {
                return $"{{{key}}}";
            }
        }
    }

    public async Task SetLanguageAsync(string language, bool reload)
    {
        await _jsRuntime.InvokeVoidAsync("setLanguageCookie", language, reload);
    }
}