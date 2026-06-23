using Microsoft.Extensions.Hosting;

namespace LVK.Bootstrapping;

public interface IApplicationBootstrapper<in T>
    where T: IHostApplicationBuilder
{
    void Bootstrap(T builder);
}