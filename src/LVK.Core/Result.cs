using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace LVK;

[PublicAPI]
public readonly struct Result<TValue, TError>
{
    private readonly TValue _value;
    private readonly TError _error;

    private Result(TValue value)
    {
        IsSuccess = true;
        _value = value;
        _error = default!;
    }

    private Result(TError error)
    {
        IsSuccess = false;
        _value = default!;
        _error = error;
    }

    public bool IsSuccess { get; }
    public bool IsError => !IsSuccess;

    public TValue Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsSuccess ? _value : throw new InvalidOperationException();
    }

    public TError Error
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => IsSuccess ? throw new InvalidOperationException() : _error;
    }

    public static implicit operator Result<TValue, TError>(TValue value) => new(value);
    public static implicit operator Result<TValue, TError>(TError error) => new(error);

    public static explicit operator TValue(Result<TValue, TError> result) => result.Value;
    public static explicit operator TError(Result<TValue, TError> result) => result.Error;

    public void Match(Action<TValue> valueHandler, Action<TError> errorHandler)
    {
        if (IsSuccess)
        {
            valueHandler(Value);
        }
        else
        {
            errorHandler(Error);
        }
    }

    public TResult Match<TResult>(Func<TValue, TResult> valueHandler, Func<TError, TResult> errorHandler)
    {
        if (IsSuccess)
        {
            return valueHandler(Value);
        }

        return errorHandler(Error);
    }

    public Result<TResult, TError> Map<TResult>(Func<TValue, Result<TResult, TError>> valueHandler)
    {
        if (IsSuccess)
        {
            return valueHandler(Value);
        }

        return Error;
    }
    public Result<TResult, TError> Map<TResult>(Func<TValue, TResult> valueHandler)
    {
        if (IsSuccess)
        {
            return valueHandler(Value);
        }

        return Error;
    }
}