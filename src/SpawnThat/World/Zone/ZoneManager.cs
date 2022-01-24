﻿using System.Collections.Generic;
using HarmonyLib;
using SpawnThat.Lifecycle;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.World.Zone;

public static class ZoneManager
{
    private static Dictionary<Vector2i, ZoneHeightmap> HeightmapsLoaded = new();
    private static Dictionary<Vector2i, ZoneSimulated> SimulatedCache = new();

    static ZoneManager()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            HeightmapsLoaded = new();
            SimulatedCache = new();
        });
    }

    public static IZone GetZone(Vector2i zoneId)
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

    [HarmonyPatch]
    private static class PatchHeightmap
    {
        [HarmonyPatch(typeof(Heightmap), nameof(Heightmap.Regenerate))]
        [HarmonyPostfix]
        private static void Record(Heightmap __instance)
        {
            var zoneId = __instance.gameObject.transform.position.GetZoneId();

            HeightmapsLoaded[zoneId] = new ZoneHeightmap(__instance);
        }

        [HarmonyPatch(typeof(Heightmap), nameof(Heightmap.OnDestroy))]
        [HarmonyPostfix]
        private static void RemoveRecord(Heightmap __instance)
        {
            var zoneId = __instance.gameObject.transform.position.GetZoneId();
            HeightmapsLoaded.Remove(zoneId);
        }
    }
}