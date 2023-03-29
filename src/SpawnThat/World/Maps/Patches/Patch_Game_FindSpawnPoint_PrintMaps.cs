using HarmonyLib;
using System.Linq;
using SpawnThat.Configuration;
using SpawnThat.Lifecycle;
using SpawnThat.Spawners.WorldSpawner.Configurations.BepInEx;
using SpawnThat.Spawners.WorldSpawner.Services;
using SpawnThat.World.Maps.Area;
using System;
using SpawnThat.Core;
using SpawnThat.Spawners.WorldSpawner.Managers;

namespace SpawnThat.World.Maps.Patches;

// TODO: Move to Debug
[HarmonyPatch(typeof(Game))]
internal class Patch_Game_FindSpawnPoint_PrintMaps
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
        try
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
                foreach (var config in WorldSpawnTemplateManager.GetTemplates())
                {
                    if (!(config.template.Enabled))
                    {
                        continue;
                    }

                    var spawnMap = WorldSpawnerSpawnMapService.GetMapOfTemplatesActiveAreas(config.id);

                    if (spawnMap is null)
                    {
                        continue;
                    }

                    ImageBuilder
                        .SetGrayscaleBiomes(MapManager.AreaMap)
                        .AddHeatZones(spawnMap)
                        .Print($"spawn_map_{config.id}_{config.template.PrefabName ?? config.template.TemplateName ?? string.Empty}");
                }
            }
        }
        catch (Exception e)
        {
            Log.LogWarning("Error while attempting to print spawn area maps. Skipping map printing.", e);
        }
    }
}
