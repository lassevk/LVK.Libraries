using Microsoft.Extensions.DependencyInjection;

namespace LasseVK.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddObjectValidation(this IServiceCollection services) => services.AddSingleton<IObjectValidationService, ObjectValidationService>();
}