namespace LVK.Validation;

// ReSharper disable NotAccessedPositionalProperty.Global

public readonly record struct ObjectValidationError(string PropertyName, string ErrorMessage);