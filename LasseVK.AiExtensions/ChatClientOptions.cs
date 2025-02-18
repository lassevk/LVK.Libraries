namespace LasseVK.AiExtensions;

public class ChatClientOptions
{
    public string? ApiKey { get; set; }

    public string? Endpoint { get; set; } = "?";

    public string? Model { get; set; }

    public virtual void Validate()
    {
        if (string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new ArgumentException("Api key is required", nameof(ApiKey));
        }
        if (string.IsNullOrWhiteSpace(Endpoint))
        {
            throw new ArgumentException("Endpoint is required", nameof(Endpoint));
        }
        if (string.IsNullOrWhiteSpace(Model))
        {
            throw new ArgumentException("Model is required", nameof(Model));
        }
    }
}