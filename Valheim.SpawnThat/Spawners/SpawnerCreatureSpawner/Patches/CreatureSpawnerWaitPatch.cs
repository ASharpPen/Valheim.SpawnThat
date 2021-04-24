using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Patches
{
    [HarmonyPatch(typeof(CreatureSpawner))]
    public static class CreatureSpawnerWaitPatch
    {
        [HarmonyPatch("UpdateSpawner")]
        [HarmonyPrefix]
        private static bool WaitForGreenLight(CreatureSpawner __instance)
        {
            if (CreatureSpawnerConfigManager.Wait)
            {
                Log.LogTrace("CreatureSpawner waiting for configs. Skipping update.");
                return false;
            }

            if (__instance.ShouldWait())
            {
                Log.LogTrace("CreatureSpawner is disabled. Skipping update.");
                return false;
            }

            return true;
        }
    }
}
