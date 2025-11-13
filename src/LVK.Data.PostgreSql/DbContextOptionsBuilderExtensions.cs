using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LVK.Data.PostgreSql;

[PublicAPI]
public static class DbContextOptionsBuilderExtensions
{
    extension(DbContextOptionsBuilder builder)
    {
        public DbContextOptionsBuilder AddNotificationSupport(IServiceProvider services)
        {
            NotificationsInterceptor interceptor = ActivatorUtilities.CreateInstance<NotificationsInterceptor>(services);
            return builder.AddInterceptors(interceptor);
        }
    }
}