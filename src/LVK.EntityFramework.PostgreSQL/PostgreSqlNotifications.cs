using Microsoft.Extensions.Options;

namespace LVK.EntityFramework.PostgreSQL;

internal class PostgreSqlNotifications : IPostgreSqlNotifications
{
    private readonly IOptions<PostgreSqlNotificationsOptions> _options;

    public PostgreSqlNotifications(IOptions<PostgreSqlNotificationsOptions> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IDisposable Listen<T>(string channel, Action<T> handler)
    {
        var listener = new PostgreSqlEventsListener<T>(_options.Value.ConnectionString, channel, handler);
        _ = listener.StartAsync();
        return listener;
    }
}