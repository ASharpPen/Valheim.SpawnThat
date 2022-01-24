using HarmonyLib;
using System.Linq;
using SpawnThat.Configuration;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;
using SpawnThat.Spawners.WorldSpawner.Services;
using SpawnThat.World.Maps.Area;

namespace SpawnThat.World.Maps.Patches;

// TODO: Move to Debug
[HarmonyPatch(typeof(Game))]
public class Patch_Game_FindSpawnPoint_PrintMaps
{
    private static bool FirstTime = true;

    static Patch_Game_FindSpawnPoint_PrintMaps()
    {
        LifecycleManager.SubscribeToWorldInit(() =>
        {
            FirstTime = true;
        });
    }

    [HarmonyPatch(nameof(Game.FindSpawnPoint))]
    [HarmonyPostfix]
    private static void PrintMaps()
    {
        if (!FirstTime)
        {
            return;
        }

        FirstTime = false;

        if (ConfigurationManager.GeneralConfig?.PrintAreaMap?.Value == true)
        {
            ImageBuilder
                .SetIds(MapManager.AreaMap)
                .Print("area_ids_map");
        }

        if (ConfigurationManager.GeneralConfig?.PrintBiomeMap?.Value == true)
        {
            ImageBuilder
                .SetBiomes(MapManager.AreaMap)
                .Print("biome_map");
        }

        if (ConfigurationManager.GeneralConfig?.PrintFantasticBeastsAndWhereToKillThem?.Value == true)
        {
            // TODO: Base on WorldSpawnerConfigurationCollection instead.

            //SpawnSystem config is only expected to have a single first layer, namely "WorldSpawner", so we just grab the first entry.
            var spawnSystemConfigs = SpawnSystemConfigurationManager
                .SpawnSystemConfig?
                .Subsections? //[*]
                .Values?
                .FirstOrDefault();

            foreach (var config in spawnSystemConfigs.Subsections.Values)
            {
                if (!config.Enabled.Value)
                {
                    continue;
                }

                var spawnMap = WorldSpawnerSpawnMapService.GetMapOfTemplatesActiveAreas(config.Index);

                if (spawnMap is null)
                {
                    continue;
                }

                ImageBuilder
                    .SetGrayscaleBiomes(MapManager.AreaMap)
                    .AddHeatZones(spawnMap)
                    .Print($"spawn_map_{config.Index}_{config.PrefabName.Value}");
            }
        }
    }
}
