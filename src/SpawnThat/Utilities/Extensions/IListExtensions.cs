using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpawnThat.Utilities.Extensions;

internal static class IListExtensions
{
    public static void AddOrReplaceByType<T, TEntry>(this IList<T> list, TEntry entry)
       where T : class
       where TEntry : class, T
    {
        var entryType = typeof(TEntry);

        int existingIndex = -1;

        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].GetType() == entryType)
            {
                existingIndex = i;
                break;
            }
        }

        if (existingIndex < 0)
        {
            list.Add(entry);
        }
        else
        {
            list[existingIndex] = entry;
        }
    }
}
