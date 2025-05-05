namespace LasseVK.Blazor;

internal class GlobalResourceProviderConfiguration
{
    public readonly List<Type> Types = [];
    public void Add(Type type)
    {
        Types.Add(type);
    }
}