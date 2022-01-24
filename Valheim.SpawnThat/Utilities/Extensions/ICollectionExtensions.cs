using System.Collections.Generic;

namespace Valheim.SpawnThat.Utilities.Extensions;

internal static class ICollectionExtensions
{
    public static void AddNullSafe<T>(this ICollection<T> collection, T newEntry)
        where T : class
    {
        if (collection is null)
        {
            return;
        }

        if (newEntry is null)
        {
            return;
        }

        collection.Add(newEntry);
    }
}
