namespace LVK.Validation;

public interface IObjectValidationService
{
    ObjectValidationResult TryValidate<T>(T obj);

    public void Validate<T>(T obj) => TryValidate(obj).ThrowIfFailure();
}