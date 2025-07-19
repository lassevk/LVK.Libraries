using System.Resources;

using Microsoft.Extensions.Options;

namespace LVK.Blazor;

internal class GlobalResourceProvider
{
    private readonly List<ResourceManager> _managers;

    public GlobalResourceProvider(IOptions<GlobalResourceProviderConfiguration> configuration)
    {
        _managers = configuration.Value.Types.Select(type => new ResourceManager(type)).ToList();
    }
    
    public string? GetString(string key)
    {
        return _managers.Select(manager => manager.GetString(key)).OfType<string>().FirstOrDefault();
    }
}