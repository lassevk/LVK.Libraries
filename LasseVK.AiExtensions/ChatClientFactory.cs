using System.ClientModel;

using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;

using OpenAI;
using OpenAI.Chat;

namespace LasseVK.AiExtensions;

public static class ChatClientFactory
{
    public static IChatClient Create(ChatClientProvider clientProvider, Action<ChatClientOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(clientProvider);

        var options = new ChatClientOptions
        {
            ApiKey = clientProvider.ApiKey,
            Endpoint = clientProvider.Endpoint,
            Model = clientProvider.DefaultModel,
        };
        configure?.Invoke(options);
        options.Validate();

        var instance = new ChatClient(options.Model, new ApiKeyCredential(options.ApiKey!), new OpenAIClientOptions
        {
            Endpoint = new Uri(options.Endpoint!),
        });

        return instance.AsChatClient().AsBuilder().UseFunctionInvocation().Build();
    }
}