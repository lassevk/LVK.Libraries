using System.Diagnostics.CodeAnalysis;

namespace LVK.Pushover;

public record PushoverNotification
{
    [SetsRequiredMembers]
    public PushoverNotification(string message)
    {
        Message = message;
    }

    public PushoverNotification()
    {
        // Do nothing here
    }

    public Guid UniqueIdentifier { get; } = Guid.NewGuid();

    public required string Message { get; init; }

    public string? TargetUser { get; init; }

    public string? TargetDevice { get; init; }

    public bool EnableHtml { get; init; }

    public string? Title { get; init; }
    public string? Url { get; init; }
    public string? UrlTitle { get; init; }

    public int? TimeToLive { get; init; }
    public DateTimeOffset? Timestamp { get; init; }

    public PushoverNotificationPriority? Priority { get; init; }
    public int? EmergencyRetryInterval { get; init; }
    public int? EmergencyExpireDuration { get; init; }

    public PushoverNotificationSound? Sound { get; init; }
    public string? CustomSound { get; init; }

    public string? AttachmentBase64 { get; init; }
    public string? AttachmentMimeType { get; init; }
}