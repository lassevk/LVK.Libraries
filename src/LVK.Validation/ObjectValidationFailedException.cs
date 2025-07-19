// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace LVK.Validation;

public class ObjectValidationFailedException : InvalidOperationException
{
    public ObjectValidationFailedException(string message, IReadOnlyCollection<ObjectValidationError> errors)
        : base(message)
    {
        Errors = errors ?? throw new ArgumentNullException(nameof(errors));
    }

    public IReadOnlyCollection<ObjectValidationError> Errors { get; }
}