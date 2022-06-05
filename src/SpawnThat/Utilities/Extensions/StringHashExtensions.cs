using System.Collections.Generic;
using System.Linq;

namespace SpawnThat.Utilities.Extensions;

public static class StringHashExtensions
{
    /// <summary>
    /// <para>Produces a stable hash.</para>
    /// <para>Based on: https://stackoverflow.com/a/36846609 </para>
    /// </summary>
    /// <remarks>Seems like Valheim uses the very same stackoverflow answer.</remarks>
    public static long Hash(this string str)
    {
        unchecked
        {
            long hash1 = 5381;
            long hash2 = hash1;

            for (int i = 0; i < str.Length && str[i] != '\0'; i += 2)
            {
                hash1 = ((hash1 << 5) + hash1) ^ str[i];
                if (i == str.Length - 1 || str[i + 1] == '\0')
                {
                    break;
                }
                hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
            }

            return hash1 + (hash2 * 1566083941);
        }
    }

    /// <summary>
    /// <para>Produces a stable hash, based on sum of <see cref="Hash"/> of each string in input.</para>
    /// <para>Deduplicates entries, and orders based on each items hash before summing.</para>
    /// </summary>
    public static long Hash(this IEnumerable<string> strs)
    {
        long hash = 0;

        var orderedHashes = strs
            .Select(x => x.Hash())
            .Distinct()
            .OrderBy(x => x);

        foreach (var hashedLocation in orderedHashes)
        {
            unchecked
            {
                hash += hashedLocation;
            }
        }

        return hash;
    }
}
