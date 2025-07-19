using System.Globalization;

using Microsoft.Extensions.Options;

namespace LVK.Pushover;

internal class Pushover : IPushover
{
    private readonly IOptions<PushoverNotificationOptions> _options;
    private readonly IHttpClientFactory _httpClientFactory;

    public Pushover(IOptions<PushoverNotificationOptions> options, IHttpClientFactory httpClientFactory)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
    }

    public async Task SendAsync(PushoverNotification notification, CancellationToken cancellationToken = default)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        HttpResponseMessage response = await client.PostAsync("https://api.pushover.net/1/messages.json", CreateContent(notification), cancellationToken);
        response.EnsureSuccessStatusCode();
    }

    private MultipartFormDataContent CreateContent(PushoverNotification evt)
    {
        var content = new MultipartFormDataContent();

        void add(string name, string? value)
        {
            if (value is not null)
            {
                content.Add(new StringContent(value), name);
            }
        }

        // Required parameters
        add("token", _options.Value.ApiToken);
        add("user", evt.TargetUser ?? _options.Value.DefaultUser ?? throw new InvalidOperationException("PushoverNotification without a target user"));
        add("message", evt.Message);

        // Optional parameters
        add("title", evt.Title);
        add("device", evt.TargetDevice);
        add("html", evt.EnableHtml ? "1" : null);

        if (evt.Priority is not null)
        {
            add("priority", ((int)evt.Priority.Value).ToString(CultureInfo.InvariantCulture));

            // if (evt.Priority == PushoverNotificationPriority.Emergency)
            // {
            //     add("retry", (evt.EmergencyRetryInterval ?? 60).ToString(CultureInfo.InvariantCulture));
            //     add("expire", (evt.EmergencyExpireDuration ?? 10800).ToString(CultureInfo.InvariantCulture));
            // }
        }

        add("sound", evt.CustomSound ?? evt.Sound?.ToString().ToLowerInvariant());
        add("url", evt.Url);
        add("url_title", evt.UrlTitle);
        add("ttl", evt.TimeToLive?.ToString(CultureInfo.InvariantCulture));
        add("timestamp", evt.Timestamp?.ToUnixTimeSeconds().ToString(CultureInfo.InvariantCulture));

        if (evt.AttachmentBase64 is null)
        {
            return content;
        }

        add("attachment_base64", evt.AttachmentBase64);
        add("attachment_type", evt.AttachmentMimeType ?? "application/octet-stream");

        return content;
    }
}