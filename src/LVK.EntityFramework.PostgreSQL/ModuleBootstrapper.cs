using LVK.Bootstrapping;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.EntityFramework.PostgreSQL;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<NotificationsCollection>();

        builder.Services.AddScoped(typeof(IPostgreSqlNotificationListener<>), typeof(PostgreSqlNotificationListener<>));
    }
}