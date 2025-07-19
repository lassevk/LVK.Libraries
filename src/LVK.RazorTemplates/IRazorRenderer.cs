using Microsoft.AspNetCore.Components;

namespace LVK.RazorTemplates;

public interface IRazorRenderer
{
    Task<string> RenderComponentAsync<T>(Action<RenderComponentConfiguration>? configure = null)
        where T : IComponent;
}