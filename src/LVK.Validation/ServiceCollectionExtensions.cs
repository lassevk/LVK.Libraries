using Microsoft.Extensions.DependencyInjection;

namespace LVK.Validation;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddObjectValidation() => services.AddSingleton<IObjectValidationService, ObjectValidationService>();
    }
}