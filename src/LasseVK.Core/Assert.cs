using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace LasseVK.Core;

public static class Assert
{
    public static void That(
        [DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression(nameof(condition))] string? callerArgumentExpression = null, [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null)
    {
        if (!condition)
        {
            throw new AssertionFailedException($"'{callerArgumentExpression}' was not true at {callerFilePath}:{callerLineNumber} in {callerMemberName}");
        }
    }
}