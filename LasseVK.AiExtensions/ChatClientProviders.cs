namespace LasseVK.AiExtensions;

public static class ChatClientProviders
{
    public static ChatClientProvider OpenAi { get; } = new()
    {
        ApiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY").ToNullIfWhiteSpace(),
        DefaultModel = "gpt-4o-mini",
        Endpoint = "https://api.openai.com/v1",
    };

    public static ChatClientProvider Openrouter { get; } = new()
    {
        ApiKey = Environment.GetEnvironmentVariable("OPENROUTER_API_KEY").ToNullIfWhiteSpace(),
        DefaultModel = "meta-llama/llama-3.1-70b-instruct:free",
        Endpoint = "https://openrouter.ai/api/v1",
    };
}