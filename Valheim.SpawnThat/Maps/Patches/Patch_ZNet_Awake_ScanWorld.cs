using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
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

            MapManager.Initialize();

            DateTimeOffset stop = DateTimeOffset.UtcNow;

            Log.LogDebug("Scanning map and labelling areas took: " + (stop - start));

            if (ConfigurationManager.GeneralConfig?.PrintAreaMap?.Value == true)
            {
                ImageBuilder
                    .SetBiomes(MapManager.AreaMap)
                    .Print("Debug", "biome_map");
            }

            if (ConfigurationManager.GeneralConfig?.PrintFantasticBeastsAndWhereToKillThem?.Value == true)
            {
                //SpawnSystem config is only expected to have a single first layer, namely "WorldSpawner", so we just grab the first entry.
                var spawnSystemConfigs = ConfigurationManager
                    .SpawnSystemConfig?
                    .Subsections? //[*]
                    .Values?
                    .FirstOrDefault();

                foreach (var config in spawnSystemConfigs.Subsections.Values)
                {
                    var spawnMap = MapManager.GetSpawnMap(config.Index);

                    ImageBuilder
                        .SetGrayscaleBiomes(MapManager.AreaMap)
                        .AddHeatZones(spawnMap)
                        .Print(Path.Combine("Debug", $"spawn_map_{config.Index}_{config.PrefabName.Value}"));
                }
            }
        }
    }
}
