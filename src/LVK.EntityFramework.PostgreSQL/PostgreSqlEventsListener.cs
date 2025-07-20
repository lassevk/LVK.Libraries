using System.Text.Json;

using Npgsql;

namespace LVK.EntityFramework.PostgreSQL;

internal class PostgreSqlEventsListener<T> : IDisposable
{
    private readonly string _connectionString;
    private readonly string _channel;
    private readonly Action<T> _handler;

    private CancellationTokenSource _cts = new();

    public PostgreSqlEventsListener(string connectionString, string channel, Action<T> handler)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        _handler = handler ?? throw new ArgumentNullException(nameof(handler));
    }

    public void Dispose()
    {
        _cts.Cancel();
    }

    public async Task StartAsync()
    {
        var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(_cts.Token);
        try
        {
            Console.WriteLine("listening on channel: " + _channel);
            NpgsqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = $"LISTEN {_channel};";
            cmd.ExecuteNonQuery();

            connection.Notification += (_, args) =>
            {
                string json = args.Payload;
                T? payload = JsonSerializer.Deserialize<T>(json);
                _handler(payload!);
            };

            while (!_cts.IsCancellationRequested)
            {
                await connection.WaitAsync(_cts.Token);
            }
        }
        catch (TaskCanceledException)
        {
            // Ignore this
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
        finally
        {
            await connection.CloseAsync();
            Console.WriteLine("stopped listening");
        }
    }
}