using System;
using System.Collections.Generic;
using System.Linq;

namespace SpawnThat.Utilities;

public static class StringExtensions
{
    private static readonly char[] Comma = new[] { ',' };
    private static readonly char[] Slash = new[] { '/', '\\' };

    public static List<string> SplitByComma(this string value, bool toUpper = false)
        => SplitBy(value, Comma, toUpper).ToList();

    internal static string[] SplitBySlash(this string value, bool toUpper = false)
        => SplitBy(value, Slash, toUpper).ToArray();

    public static IEnumerable<string> SplitBy(this string value, char[] chars, bool toUpper = false)
    {
        var split = value.Split(chars, StringSplitOptions.RemoveEmptyEntries);

        if ((split?.Length ?? 0) == 0)
        {
            return Enumerable.Empty<string>();
        }

        return split.Select(Clean);

        string Clean(string x)
        {
            var result = x.Trim();
            if (toUpper)
            {
                return result.ToUpperInvariant();
            }
            return result;
        }
    }

    public static bool IsEmpty(this string s)
    {
        return string.IsNullOrWhiteSpace(s);
    }

    public static bool IsNotEmpty(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return false;
        }
        return true;
    }
}
