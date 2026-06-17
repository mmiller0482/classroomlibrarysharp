namespace ClassroomLibrary.Utils;

/// <summary>Shared normalization helpers for user-entered text.</summary>
public static class StringUtils
{
    /// <summary>Trims surrounding whitespace and converts null input to an empty string.</summary>
    public static string CleanInputString(string? value) => value?.Trim() ?? string.Empty;
}
