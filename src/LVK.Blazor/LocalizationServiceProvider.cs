using System.Resources;

using Microsoft.JSInterop;

namespace LVK.Blazor;

internal class LocalizationServiceProvider : ILocalizationServiceProvider
{
    private readonly IJSRuntime _jsRuntime;
    private readonly GlobalResourceProvider? _globalResourceProvider;

    public LocalizationServiceProvider(IJSRuntime jsRuntime, GlobalResourceProvider? globalResourceProvider = null)
    {
        _jsRuntime = jsRuntime ?? throw new ArgumentNullException(nameof(jsRuntime));
        _globalResourceProvider = globalResourceProvider;
    }

    public ILocalizationService GetService(Type type)
    {
        return new LocalizationService(_jsRuntime, _globalResourceProvider, new ResourceManager(type));
    }
}