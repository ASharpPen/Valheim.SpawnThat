using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.WorldMap
{
    [HarmonyPatch(typeof(ZNet))]
    internal static class Patch_ZNet_Awake_ScanWorld
    {
        private static AreaMap Map;

        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void ScanWorld()
        {
            Log.LogInfo("Scanning Map");

            DateTimeOffset start = DateTimeOffset.UtcNow;

            Map = new AreaMap(new WorldGeneratorAreaProvider(), 10000);
            Map.Init(10000);
            Map.FirstScan();
            Map.MergeLabels();
            Map.Build();

            DateTimeOffset stop = DateTimeOffset.UtcNow;

            Log.LogInfo("Map scan took: " + (stop - start));

            MapPrinter.PrintAreaMap(Map);
        }
    }
}
