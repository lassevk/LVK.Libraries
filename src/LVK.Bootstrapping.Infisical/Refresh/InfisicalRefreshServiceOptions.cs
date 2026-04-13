using LVK.Bootstrapping.Infisical.Api;

namespace LVK.Bootstrapping.Infisical.Refresh;

internal class InfisicalRefreshServiceOptions
{
    public required long RefreshIntervalSeconds { get; set; }
    public required Secret[] Secrets { get; set; }
    public required InfisicalService Service { get; set; }
}