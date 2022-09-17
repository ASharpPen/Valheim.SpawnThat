using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SpawnThat.Utilities.Extensions;

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

        if (newEntry is UnityEngine.Object unityObj &&
            !unityObj)
        {
            return;
        }

        collection.Add(newEntry);
    }

    public static string Join(this ICollection<float> list) => list.Join(x => x.ToString(CultureInfo.InvariantCulture));
    public static string Join(this ICollection<double> list) => list.Join(x => x.ToString(CultureInfo.InvariantCulture));

    public static string Join<T>(this ICollection<T> list)
    {
        if (list is null ||
            list.Count == 0)
        {
            return "";
        }

        return string.Join(", ", list);
    }

    public static string Join<T>(this ICollection<T> list, Func<T, string> tostring)
    {
        if (list is null ||
            list.Count == 0)
        {
            return string.Empty;
        }

        return string.Join(", ", list.Select(x => tostring(x)));
    }

    public static string Join<T, K>(this ICollection<T> list, Func<T, K> selector)
    {
        if (list is null ||
            list.Count == 0)
        {
            return string.Empty;
        }

        return string.Join(", ", list.Select(x => selector(x)));
    }
}
