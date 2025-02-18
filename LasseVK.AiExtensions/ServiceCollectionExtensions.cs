using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LasseVK.AiExtensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKeyedChatClient(this IServiceCollection services, object serviceKey, ChatClientProvider clientProvider, Action<ChatClientOptions>? configure)
    {
        services.TryAddKeyedSingleton(serviceKey, ChatClientFactory.Create(clientProvider, configure));
        return services;
    }

    public static IServiceCollection AddChatClient(this IServiceCollection services, ChatClientProvider clientProvider, Action<ChatClientOptions> configure)
    {
        services.TryAddSingleton(ChatClientFactory.Create(clientProvider, configure));
        return services;
    }
}