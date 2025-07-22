using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

namespace LVK;

[PublicAPI]
public static class Assume
{
    [Conditional("DEBUG")]
    public static void That(
        [DoesNotReturnIf(false)] bool condition, [CallerArgumentExpression(nameof(condition))] string? callerArgumentExpression = null, [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null)
    {
        if (!condition)
        {
            Debug.WriteLine($"assumption '{callerArgumentExpression}' did not hold at {callerFilePath}:{callerLineNumber} in {callerMemberName}");
        }
    }
}