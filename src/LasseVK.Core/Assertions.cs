using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using JetBrains.Annotations;

// ReSharper disable EntityNameCapturedOnly.Global
// ReSharper disable InconsistentNaming

namespace LasseVK;

[PublicAPI]
public static class Assertions
{
    [ContractAnnotation("assumption:false => halt")]
    public static void assume(
        [DoesNotReturnIf(false)] bool assumption, [CallerArgumentExpression(nameof(assumption))] string? assertionExpression = null, [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null)
    {
        if (!assumption)
        {
            Debug.WriteLine($"Assumption '{assertionExpression}' failed, in {callerMemberName} at {callerFilePath}:{callerLineNumber}");
        }
    }

    [ContractAnnotation("assertion:false => halt")]
    public static void assert(
        [DoesNotReturnIf(false)] bool assertion, [CallerArgumentExpression(nameof(assertion))] string? assertionExpression = null, [CallerFilePath] string? callerFilePath = null,
        [CallerLineNumber] int callerLineNumber = 0, [CallerMemberName] string? callerMemberName = null)
    {
        if (!assertion)
        {
            throw new InvalidOperationException($"Assertion '{assertionExpression}' failed, in {callerMemberName} at {callerFilePath}:{callerLineNumber}");
        }
    }
}