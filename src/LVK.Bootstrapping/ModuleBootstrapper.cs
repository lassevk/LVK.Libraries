using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

internal class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        object hostKey = new();
        ServiceDescriptor hostDescriptor = builder.Services.FirstOrDefault(sd => sd.ServiceType == typeof(IHost))!;
        builder.Services.Remove(hostDescriptor);
        builder.Services.AddKeyedSingleton<IHost>(hostKey, (sp, _) => (IHost)hostDescriptor.ImplementationFactory!(sp));
        builder.Services.AddSingleton<IHost, HostDecorator>(sp => new HostDecorator(sp.GetRequiredKeyedService<IHost>(hostKey)));
    }
}