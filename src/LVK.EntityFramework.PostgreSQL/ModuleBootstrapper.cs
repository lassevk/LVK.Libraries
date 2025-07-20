using LVK.Hosting;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LVK.EntityFramework.PostgreSQL;

public class ModuleBootstrapper : IModuleBootstrapper
{
    public void Bootstrap(IHostApplicationBuilder builder)
    {
        builder.Services.AddTransient<IPostgreSqlNotifications, PostgreSqlNotifications>();
        builder.Services.AddScoped<NotificationsCollection>();
    }
}