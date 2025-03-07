namespace LasseVK.Validation;

public interface IObjectValidator<in T>
{
    ObjectValidationResult TryValidate(T obj);

    public void Validate(T obj) => TryValidate(obj).ThrowIfFailure();
}