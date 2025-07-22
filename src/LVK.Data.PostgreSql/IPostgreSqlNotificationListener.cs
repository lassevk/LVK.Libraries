using System.Text.Json;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;

namespace LVK.Data.PostgreSql;

[PublicAPI]
public interface IPostgreSqlNotificationListener<T>
    where T : DbContext
{
    void AddListener(string channel, Action<string> callback);

    void AddListener<TPayload>(string channel, Action<TPayload> callback)
    {
        AddListener(channel, json =>
        {
            TPayload payload = JsonSerializer.Deserialize<TPayload>(json) ?? default!;
            callback(payload);
        });
    }

    Task ListenAsync(CancellationToken cancellationToken);
}