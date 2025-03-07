namespace LasseVK.Validation;

public readonly record struct ObjectValidationResult(IReadOnlyCollection<ObjectValidationError> Errors)
{
    public bool IsSuccess => Errors.Count == 0;
    public bool IsFailure => !IsSuccess;

    public void ThrowIfFailure()
    {
        if (IsFailure)
        {
            throw new ObjectValidationFailedException($"Object validation failed with {Errors.Count} error{(Errors.Count == 1 ? "" : "s")}",  Errors);
        }
    }
}