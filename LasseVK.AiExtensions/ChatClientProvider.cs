namespace LasseVK.AiExtensions;

public class ChatClientProvider
{
    public required string? ApiKey { get; init; }
    public required string? DefaultModel { get; init; }
    public required string? Endpoint { get; init; }
}