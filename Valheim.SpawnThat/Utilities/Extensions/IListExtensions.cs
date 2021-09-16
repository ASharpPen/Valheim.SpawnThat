using System;
using System.Collections.Generic;

namespace Valheim.SpawnThat.Utilities.Extensions
{
    public static class IListExtensions
    {
        private static Random _random;

        private static Random DefaultRandom => _random ??= new();

        public static T Random<T>(this IList<T> list, Random random = null)
        {
            if (list is null)
            {
                return default;
            }

            random ??= DefaultRandom;

            return list[random.Next(list.Count)];
        }
    }
}
