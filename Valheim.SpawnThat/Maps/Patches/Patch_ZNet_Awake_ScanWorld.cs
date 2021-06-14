using HarmonyLib;
using System;
using System.IO;
using System.Linq;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Maps;
using Valheim.SpawnThat.Maps.Managers;

namespace Valheim.SpawnThat.Maps.Patches
{
    [HarmonyPatch(typeof(ZNet))]
    internal static class Patch_ZNet_Awake_ScanWorld
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void ScanWorld()
        {
            Log.LogDebug("Scanning map for biomes..");

            DateTimeOffset start = DateTimeOffset.UtcNow;

            MapManager.Initialize();

            DateTimeOffset stop = DateTimeOffset.UtcNow;

            Log.LogDebug("Scanning map and assigning id's to areas took: " + (stop - start));
        }
    }
}
