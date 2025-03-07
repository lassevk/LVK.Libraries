namespace LasseVK.Validation;

public interface ISelfValidatingObject
{
    ObjectValidationResult TryValidate();

    public void Validate() => TryValidate().ThrowIfFailure();
}