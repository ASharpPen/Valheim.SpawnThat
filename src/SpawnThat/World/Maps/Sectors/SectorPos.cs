using System;
using SpawnThat.Lifecycle;
using UnityEngine;

namespace SpawnThat.World.Maps.Sectors;

/// <summary>
/// Coordinates for sector position in grid.
/// </summary>
internal readonly struct SectorPos(int x, int y)
{
    private static int WorldSize = 0;

    public int X { get; } = x;

    public int Y { get; } = y;

    static SectorPos()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            WorldSize = 0;
        });

        LifecycleManager.OnBiomesLoaded += () =>
        {
            WorldSize = ZNet.World.m_biomeData.Size;
        };
    }

    public static SectorPos FromWorld(in Vector3 pos)
    {
        int x = Math.Min(WorldSize - 1, Math.Max(0, AltBiomeWorldData.WorldSpaceToMapSpace(pos.x)));
        int y = Math.Min(WorldSize - 1, Math.Max(0, AltBiomeWorldData.WorldSpaceToMapSpace(pos.y)));

        return new(x, y);
    }
}