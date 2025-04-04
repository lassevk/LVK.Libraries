using Microsoft.Extensions.DependencyInjection;

namespace LasseVK.RazorTemplates;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRazorRenderer(this IServiceCollection services) => services.AddTransient<IRazorRenderer, RazorRenderer>();
}