using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LVK.EntityFramework.PostgreSQL;

[PublicAPI]
public static class DbContextOptionsBuilderExtensions
{
    public static DbContextOptionsBuilder AddNotificationSupport(this DbContextOptionsBuilder builder, IServiceProvider services)
    {
        NotificationsInterceptor interceptor = ActivatorUtilities.CreateInstance<NotificationsInterceptor>(services);
        return builder.AddInterceptors(interceptor);
    }
}