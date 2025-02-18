namespace LasseVK.AiExtensions;

internal static class StringExtensions
{
    public static string? ToNullIfWhiteSpace(this string? input) => string.IsNullOrWhiteSpace(input) ? null : input;
}