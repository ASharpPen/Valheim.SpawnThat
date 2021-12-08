using HarmonyLib;
using System.IO;
using System.Linq;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Maps.Managers;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Maps.Patches;

[HarmonyPatch(typeof(Game))]
public class Patch_Game_FindSpawnPoint_PrintMaps
{
    private static bool FirstTime = true;

    static Patch_Game_FindSpawnPoint_PrintMaps()
    {
        StateResetter.Subscribe(() =>
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
                .Print("Debug", "area_ids_map");
        }

        if (ConfigurationManager.GeneralConfig?.PrintBiomeMap?.Value == true)
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

            if (spawnSystemConfigs is null)
            {
                return;
            }

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
