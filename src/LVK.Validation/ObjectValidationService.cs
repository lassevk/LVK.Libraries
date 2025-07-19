using Microsoft.Extensions.DependencyInjection;

namespace LVK.Validation;

internal class ObjectValidationService : IObjectValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ObjectValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public ObjectValidationResult TryValidate<T>(T obj)
    {
        if (obj is ISelfValidatingObject selfValidatingObject)
        {
            return selfValidatingObject.TryValidate();
        }

        IObjectValidator<T>? validator = _serviceProvider.GetService<IObjectValidator<T>>();
        return validator?.TryValidate(obj) ?? new ObjectValidationResult([new("this", $"No validator found for type {typeof(T).Name}")]);
    }
}