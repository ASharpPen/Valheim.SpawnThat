using HarmonyLib;
using System;
using SpawnThat.Core;

namespace SpawnThat.World.Maps.Patches;

[HarmonyPatch(typeof(ZNet))]
internal static class Patch_ZNet_Awake_ScanWorld
{
    [HarmonyPatch(nameof(ZNet.Awake))]
    [HarmonyPostfix]
    private static void ScanWorld()
    {
        if (WorldGenerator.instance is not null)
        {
            Log.LogDebug("Scanning map for biomes..");

            DateTimeOffset start = DateTimeOffset.UtcNow;

            MapManager.Initialize();

            DateTimeOffset stop = DateTimeOffset.UtcNow;

            Log.LogDebug("Scanning map and assigning id's to areas took: " + (stop - start));
        }
    }

    [HarmonyPatch(nameof(ZNet.RPC_PeerInfo))]
    [HarmonyPostfix]
    private static void ScanJoinedWorld()
    {
        if (WorldGenerator.instance is not null)
        {
            Log.LogDebug("Scanning map for biomes..");

            DateTimeOffset start = DateTimeOffset.UtcNow;

            MapManager.Initialize();

            DateTimeOffset stop = DateTimeOffset.UtcNow;

            Log.LogDebug("Scanning map and assigning id's to areas took: " + (stop - start));
        }
    }
}
