using HarmonyLib;
using System.Collections;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;

namespace Valheim.SpawnThat.Reset
{
    [HarmonyPatch(typeof(Game))]
    public static class GameStartupPatch
    {
        private static bool FirstTime = true;

        static GameStartupPatch()
        {
            StateResetter.Subscribe(() =>
            {
                FirstTime = true;
            });
        }

        [HarmonyPatch("FindSpawnPoint")]
        [HarmonyPostfix]
        private static void LoadConfigs(Game __instance)
        {
            if (FirstTime)
            {
                FirstTime = false;
                _ = __instance.StartCoroutine(ReleaseConfigs());
            }
        }

        public static IEnumerator ReleaseConfigs()
        {
            Log.LogDebug("Starting early delay for config application.");

            yield return new WaitForSeconds(2);

            Log.LogDebug("Finished early delay for config application.");
            SpawnSystemConfigManager.Wait = false;
            CreatureSpawnerConfigManager.Wait = false;
        }
    }
}