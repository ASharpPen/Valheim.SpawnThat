using HarmonyLib;
using System;
using Valheim.SpawnThat.Utilities;

namespace Valheim.SpawnThat.ServerSide
{
    [HarmonyPatch(typeof(CreatureSpawner))]
    internal static class Patch_CreatureSpawner_Awake_ClaimOwnership
    {
        [HarmonyPatch(nameof(CreatureSpawner.Awake))]
        [HarmonyPostfix]
        private static void ClaimOwnership(CreatureSpawner __instance)
        {
            if (!__instance.m_nview.IsOwner())
            {
                if (ZNet.instance.IsServer())
                {
                    Log.LogInfo("Claiming spawner.");
                    __instance.m_nview.ClaimOwnership();
                }
            }
        }

        [HarmonyPatch(nameof(CreatureSpawner.UpdateSpawner))]
        [HarmonyPrefix]
        private static void KeepClaiming(CreatureSpawner __instance)
        {
            if (!__instance.m_nview.IsOwner())
            {
                if (ZNet.instance.IsServer())
                {
                    Log.LogInfo("Claiming creature spawner.");
                    __instance.m_nview.ClaimOwnership();
                }
            }
        }
    }
}
