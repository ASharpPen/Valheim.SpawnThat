using HarmonyLib;
using System;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.ServerSide
{
    [HarmonyPatch]
    internal static class Patch_SpawnSystem_Awake_ClaimOwnership
    {
        [HarmonyPatch(typeof(SpawnSystem))]
        [HarmonyPatch(nameof(SpawnSystem.Awake))]
        [HarmonyPostfix]
        private static void ClaimOwnership(SpawnSystem __instance)
        {
            if (__instance.transform.position.WithinSquare(1000000, 1000000, 200))
            {
                // Ignore server spawners way outside map range.
                return;
            }

            if (!__instance.m_nview.IsOwner())
            {
                if (ZNet.instance.IsServer())
                {
                    Log.LogInfo($"Claiming spawner at {__instance.transform.position}.");
                    __instance.m_nview.ClaimOwnership();
                }
            }
        }

        /// <summary>
        /// Mine! No takebacksies!
        /// </summary>
        [HarmonyPatch(typeof(SpawnSystem))]
        [HarmonyPatch(nameof(SpawnSystem.UpdateSpawning))]
        [HarmonyPrefix]
        private static void KeepClaiming(SpawnSystem __instance)
        {
            if (!__instance.m_nview.IsOwner())
            {
                if (__instance.transform.position.WithinSquare(1000000, 1000000, 200))
                {
                    // Ignore server spawners way outside map range.
                    return;
                }

                if (ZNet.instance.IsServer())
                {
                    Log.LogInfo($"Spawner owner '{__instance.m_nview.GetZDO().m_uid}' is not server '{ZDOMan.instance.GetMyID()}'");
                    Log.LogInfo($"Claiming spawner at {__instance.transform.position}.");
                    __instance.m_nview.ClaimOwnership();
                }
            }
        }
    }
}
