using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LVK.RazorTemplates;

internal class RazorRenderer : IRazorRenderer
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILoggerFactory _loggerFactory;

    public RazorRenderer(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }
    
    public async Task<string> RenderComponentAsync<T>(Action<RenderComponentConfiguration>? configure = null)
        where T : IComponent
    {
        var configuration = new RenderComponentConfiguration();
        configure?.Invoke(configuration);

        if (!configuration.UseScopedServices)
        {
            return await RenderComponentAsync<T>(configuration, _serviceProvider);
        }

        using IServiceScope scope = _serviceProvider.CreateScope();
        return await RenderComponentAsync<T>(configuration, scope.ServiceProvider);
    }

    private async Task<string> RenderComponentAsync<T>(RenderComponentConfiguration configuration, IServiceProvider serviceProvider)
        where T : IComponent
    {
        await using var renderer = new HtmlRenderer(serviceProvider, _loggerFactory);
        return await renderer.Dispatcher.InvokeAsync(async () =>
        {
            ParameterView parameters = configuration.Parameters.Any() ? ParameterView.FromDictionary(configuration.Parameters) : ParameterView.Empty;
            return (await renderer.RenderComponentAsync<T>(parameters)).ToHtmlString();
        });
    }
}