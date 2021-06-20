using HarmonyLib;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    /// <summary>
    /// The base code only checks distance based on BaseAI component of prefab, which is currently loaded in.
    /// This means any check with a non-AI containing prefab, will return as false.
    /// 
    /// This code attempts to fix this, by intercepting the usual call, and replacing the solution with
    /// a check for position of zdo's within distance, which have the same stable hash as the tested prefab.
    /// 
    /// While this may be an expensive call to check exhaustively, the default UpdateSpawnList will at most
    /// run it once pr entity to spawn.
    /// Caching is almost pointless, as the centerpoint and min distance can/will change per check.
    /// 
    /// Future optimization would be to identify the maximum possible range to search, generate a single
    /// SpawnCounter for the session which can accommodate this. As well as general optimization to the 
    /// search logic inside that class itself.
    /// </summary>
    [HarmonyPatch(typeof(SpawnSystem))]
    internal static class SpawnSystemHaveInstanceInRangePatch
    {
        [HarmonyPatch(nameof(SpawnSystem.HaveInstanceInRange))]
        [HarmonyPrefix]
        private static bool FixSpawnsInRangeForNonAI(GameObject prefab, Vector3 centerPoint, float minDistance, ref bool __result)
        {
            //Fish and birds all seem to have this tag assigned. It will be counted by the standard method.
            if (prefab.tag == "spawned")
            {
                return true;
            }

            BaseAI baseAI = PrefabCache.GetBaseAI(prefab);

            if (baseAI && baseAI is not null)
            {
                return true;
            }

            SpawnCounter counter = new SpawnCounter(centerPoint, (int)minDistance);
            __result = counter.HasAnyInRange(prefab);

            return false;
        }
    }
}
