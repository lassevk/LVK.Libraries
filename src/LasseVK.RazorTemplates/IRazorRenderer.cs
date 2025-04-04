using Microsoft.AspNetCore.Components;

namespace LasseVK.RazorTemplates;

public interface IRazorRenderer
{
    Task<string> RenderComponentAsync<T>(Action<RenderComponentConfiguration>? configure = null)
        where T : IComponent;
}