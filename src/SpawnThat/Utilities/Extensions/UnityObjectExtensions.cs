using System.Text.RegularExpressions;

namespace SpawnThat.Utilities.Extensions;

public static class UnityObjectExtensions
{
    private static Regex NameRegex = new Regex(@"^[^$(]*(?=$|[(])", RegexOptions.Compiled);

    public static bool IsNull(this UnityEngine.Object obj)
    {
        if (obj == null || !obj)
        {
            return true;
        }

        return false;
    }

    public static bool IsNotNull(this UnityEngine.Object obj)
    {
        if (obj != null && obj)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Unity object null safe ".name".
    /// </summary>
    /// <returns><c>obj</c>.name or empty string if obj is null.</returns>
    public static string GetName(this UnityEngine.Object obj)
        => obj.IsNotNull()
        ? obj.name
        : string.Empty;

    public static string GetCleanedName(this UnityEngine.Object obj, bool toUpper = false)
    {
        if (obj.IsNull())
        {
            return null;
        }

        var match = NameRegex.Match(obj.name);

        if (!match.Success)
        {
            return null;
        }

        string cleanedName = match
            .Value
            .Trim();
        /*
        if (obj.IsNull())
        {
            return null;
        }

        string cleanedName = obj.name
            .Split(new char[] { '(' }, System.StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault()?
            .Trim();

        cleanedName = string.IsNullOrWhiteSpace(cleanedName)
            ? obj.name
            : cleanedName;
        */

        if (toUpper)
        {
            cleanedName = cleanedName?.ToUpperInvariant();
        }

        return cleanedName;
    }
}
