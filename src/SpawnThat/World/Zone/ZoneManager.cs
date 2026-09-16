using System.Collections.Generic;
using HarmonyLib;
using SpawnThat.Lifecycle;
using SpawnThat.Utilities.Extensions;

#if DEBUG && FALSE
using SpawnThat.Debugging.Gizmos;
using UnityEngine;
#endif


namespace SpawnThat.World.Zone;

public static class ZoneManager
{
    internal static Dictionary<Vector2s, ZoneHeightmap> HeightmapsLoaded = new();
    internal static Dictionary<Vector2s, ZoneSimulated> SimulatedCache = new();

    static ZoneManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            HeightmapsLoaded = new();
            SimulatedCache = new();
        });
    }

    public static IZone GetZone(Vector2s zoneId)
    {
        if (HeightmapsLoaded.TryGetValue(zoneId, out var cached))
        {
            return cached;
        }

        if (SimulatedCache.TryGetValue(zoneId, out var cachedSimulated))
        {
            return cachedSimulated;
        }

        return SimulatedCache[zoneId] = new ZoneSimulated(zoneId);
    }
}
