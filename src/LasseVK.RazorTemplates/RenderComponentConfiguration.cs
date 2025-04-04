using System.Reflection;
using System.Runtime.CompilerServices;

namespace LasseVK.RazorTemplates;

public class RenderComponentConfiguration
{
    internal Dictionary<string, object?> Parameters { get; } = new();
    internal Action<IServiceProvider>? ConfigureServices { get; set; }
    internal bool UseScopedServices { get; set; }

    public RenderComponentConfiguration WithParameter(string name, object? value)
    {
        Parameters[name] = value;
        return this;
    }

    public RenderComponentConfiguration WithParameters(object parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);

        foreach (PropertyInfo property in parameters.GetType().GetProperties())
        {
            WithParameter(property.Name, property.GetValue(parameters));
        }

        return this;
    }

    public RenderComponentConfiguration WithScopedServices(Action<IServiceProvider>? configure = null)
    {
        UseScopedServices = true;
        ConfigureServices = configure;
        return this;
    }
}