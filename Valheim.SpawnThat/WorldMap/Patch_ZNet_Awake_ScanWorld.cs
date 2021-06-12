using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.WorldMap
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

            MapManager.AreaMap = new AreaMap(new WorldGeneratorAreaProvider(), 10000);
            MapManager.AreaMap.Complete();

            DateTimeOffset stop = DateTimeOffset.UtcNow;

            Log.LogDebug("Scanning map and labelling areas took: " + (stop - start));

            if (ConfigurationManager.GeneralConfig?.PrintAreaMap?.Value == true)
            {
                MapPrinter.PrintAreaMap(MapManager.AreaMap);
            }
        }
    }
}
