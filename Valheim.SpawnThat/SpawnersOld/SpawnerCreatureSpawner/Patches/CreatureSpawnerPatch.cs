using System;
using HarmonyLib;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Patches
{
    [Obsolete("Being replaced by LocalSpawner patches")]
    [HarmonyPatch(typeof(CreatureSpawner))]
    public static class CreatureSpawnerPatch
    {
        [HarmonyPatch("Awake")]
        [HarmonyPostfix]
        private static void SetupConfigsOnAwake(CreatureSpawner __instance)
        {
            if (ZNet.instance is null)
            {
                CreatureSpawnerConfigManager.ApplyConfigs(__instance);
            }
            else if (ZNet.instance.IsServer())
            {
                CreatureSpawnerConfigManager.ApplyConfigs(__instance);
            }
        }

        [HarmonyPatch("UpdateSpawner")]
        [HarmonyPrefix]
        private static void SetupConfigsOnUpdate(CreatureSpawner __instance)
        {
            CreatureSpawnerConfigManager.ApplyConfigsIfMissing(__instance);
        }
    }
}
