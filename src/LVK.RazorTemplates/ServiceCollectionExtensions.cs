using Microsoft.Extensions.DependencyInjection;

namespace LVK.RazorTemplates;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddRazorRenderer() => services.AddTransient<IRazorRenderer, RazorRenderer>();
    }
}