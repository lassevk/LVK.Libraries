using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LVK.EntityFramework.PostgreSQL;

public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder AddNotificationSupport(this DbContextOptionsBuilder builder, IServiceProvider services)
    {
        NotificationsCollection notifications = services.GetRequiredService<NotificationsCollection>();
        var interceptor = new PostgreSqlNotificationsInterceptor(notifications);
        builder.AddInterceptors(interceptor);

        return builder;
    }
}