namespace LasseVK;

public static class StringExtensions
{
    public static string? ToNullIfEmpty(this string? str) => string.IsNullOrEmpty(str) ? null : str;
    public static string? ToNullIfWhiteSpace(this string? str) => string.IsNullOrWhiteSpace(str) ? null : str;
}