#define TEST

using HarmonyLib;
using System;
using System.Collections.Generic;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.ServerSide
{
    [HarmonyPatch]
    internal static class Patch_SpawnSystem_Awake_ClaimOwnership
    {
        [HarmonyPatch(typeof(ZDOMan), nameof(ZDOMan.CreateSyncList))]
        [HarmonyPostfix]
        private static void ClaimZDOsBeingSent(List<ZDO> toSync)
        {
            // Lets take a gander, and grab whats ours.

            var hash = GameConstants.SpawnSystemPrefabHash;
            var id = GameConstants.ServerID;

            int claims = 0;

            for(int i = 0; i < toSync.Count; ++i)
            {
                ZDO current = toSync[i];
                if (current.m_prefab == hash && current.m_owner != id)
                {
                    current.SetOwner(id);
                    claims++;
                }
            }

#if TEST && DEBUG
            if (claims > 0)
            {
                Log.LogDebug($"Claimed {claims} SpawnSystem zdo's");
            }
#endif
        }

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

            __instance.StartCoroutine(
                () =>
                {
                    Log.LogInfo($"Claiming spawner at {__instance.transform.position}.");
                    __instance.m_nview.ClaimOwnership();
                },
                TimeSpan.FromMilliseconds(1));
        }

        [HarmonyPatch(typeof(SpawnSystem))]
        [HarmonyPatch(nameof(SpawnSystem.UpdateSpawning))]
        [HarmonyPrefix]
        private static void KeepClaiming(SpawnSystem __instance)
        {
            if (__instance.m_nview.IsOwner())
            {
                if (__instance.transform.position.WithinSquare(1000000, 1000000, 200))
                {
                    // Ignore server spawners way outside map range.
                    return;
                }

                Log.LogInfo($"Spawner owner '{__instance.m_nview.GetZDO().m_uid}' is not server '{ZDOMan.instance.GetMyID()}'");
                Log.LogInfo($"Claiming spawner at {__instance.transform.position}.");
                __instance.m_nview.ClaimOwnership();
            }
        }
    }
}
