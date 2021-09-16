#define TEST

using HarmonyLib;
using System;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class SpawnSystemPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void SetupConfigs(SpawnSystem __instance, Heightmap ___m_heightmap)
        {
            if (__instance.transform.position.WithinSquare(1000000, 1000000, 200))
            {
                // Ignore server spawners way outside map range.
                return;
            }

            if(ZNet.instance is null)
            {
#if TEST && DEBUG
                Log.LogTrace("SpawnSystem.Awake -> ApplyConfigs");
#endif
                SpawnSystemConfigManager.ApplyConfigs(__instance, ___m_heightmap);
            }
            else if(ZNet.instance.IsServer())
            {
#if TEST && DEBUG
                Log.LogTrace("SpawnSystem.Awake (server) -> ApplyConfigs");
#endif
                SpawnSystemConfigManager.ApplyConfigs(__instance, ___m_heightmap);
            }
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.Normal)]
        private static void ApplyConfigsIfMissing(SpawnSystem __instance, Heightmap ___m_heightmap, bool eventSpawners)
        {
            if(eventSpawners)
            {
                return;
            }

            try
            {
#if TEST && DEBUG
                Log.LogTrace("UpdateSpawnList -> ApplyConfigsIfMissing");
#endif
                SpawnSystemConfigManager.ApplyConfigsIfMissing(__instance, ___m_heightmap);
            }
            catch (Exception e)
            {
                Log.LogError(e.ToString());
            }
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyPrefix]
        [HarmonyPriority(Priority.LowerThanNormal)]
        private static bool WaitForGreenLight(SpawnSystem __instance, Heightmap ___m_heightmap,  bool eventSpawners)
        {
            try
            {
                // Ignore event spawners. Let them do what they do.
                if (eventSpawners)
                {
                    return true;
                }

                if (SpawnSystemConfigManager.Wait && !eventSpawners)
                {
                    Log.LogTrace("SpawnSystem waiting for configs. Skipping update.");
                    return false;
                }

                if (__instance.ShouldWait())
                {
                    Log.LogTrace($"SpawnSystem at {__instance?.gameObject?.transform?.position} is disabled. Skipping update.");
                    return false;
                }
            }
            catch(Exception e)
            {
                Log.LogError($"Error while waiting for green light to spawn.", e);
            }

            return true;
        }
    }
}
