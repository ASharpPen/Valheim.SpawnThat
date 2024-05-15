using System;
using System.Collections.Generic;

namespace SpawnThat.Utilities.Extensions;

internal static class EnumExtensions
{
    /// <summary>
    /// Splits <paramref name="enumVal"/> into a list of each enum value present in it.
    /// Intended to output binary flagged values.
    /// </summary>
    public static List<TEnum> Split<TEnum>(this TEnum enumVal) 
        where TEnum : struct, Enum
    {
        var val = Convert.ToInt32(enumVal);

        List<TEnum> result = new List<TEnum>();

        foreach (TEnum value in Enum.GetValues(typeof(TEnum)))
        {
            if ((val & Convert.ToInt32(value)) > 0)
            {
                result.Add(value);
            }
        }

        return result;
    }

    public static List<Heightmap.Biome> Split(this Heightmap.Biome biomeMask)
    {
        List<Heightmap.Biome> result = new List<Heightmap.Biome>();

        foreach (Heightmap.Biome value in Enum.GetValues(typeof(Heightmap.Biome)))
        {
            if (value == Heightmap.Biome.All)
            {
                if (biomeMask == Heightmap.Biome.All)
                {
                    result.Add(value);
                }
            }
            else if ((biomeMask & value) > 0)
            {
                result.Add(value);
            }
        }

        return result;
    }
}
