using System;
using UnityEngine;

namespace SpawnThat.World.Maps.Images.Colors;

internal readonly struct HeatColorMapper : IColorMapper<int>
{
    public static HeatColorMapper FromIntMap() => new HeatColorMapper();

    public Color Map(int cell)
    {
        cell = Math.Min(255, Math.Max(0, cell));

        return new Color32(255, (byte)(255 - cell), (byte)(255 - cell), 255);
    }
}
