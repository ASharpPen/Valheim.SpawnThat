using HarmonyLib;
using UnityEngine;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    internal static class SpawnSystemGetNrOfInstancesPatch
    {
        [HarmonyPatch(nameof(SpawnSystem.GetNrOfInstances), new[] { typeof(GameObject), typeof(Vector3), typeof(float), typeof(bool), typeof(bool) })]
        [HarmonyPrefix]
        private static bool FixSpawnCount(GameObject prefab, Vector3 center, float maxRange, ref int __result)
        {
            //Fish and birds all seem to have this tag assigned. It will be counted by the standard method.
            if(prefab.tag == "spawned")
            {
                return true;
            }

            BaseAI baseAI = PrefabCache.GetBaseAI(prefab);

            if (baseAI && baseAI is not null)
            {
                return true;
            }

            if (center == Vector3.zero && maxRange == 0)
            {
                // Only use cached service when using defaults.

                var counter = SpawnSessionManager.Instance.GetService<SpawnCounter>();

                if (counter is not null)
                {
                    __result = counter.CountInstancesInRange(prefab);
                    return false;
                }
            }
            else
            {
                var counter = new SpawnCounter(center, (int)maxRange);
                __result = counter.CountInstancesInRange(prefab);

                return false;
            }

            return true;
        }
    }
}
