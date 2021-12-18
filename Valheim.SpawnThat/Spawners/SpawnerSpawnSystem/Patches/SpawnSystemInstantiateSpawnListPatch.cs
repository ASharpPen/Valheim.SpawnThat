using System;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    internal static class SpawnSystemInstantiateSpawnListPatch
    {
        private static bool HasInstantiatedSpawnLists;

        private static List<GameObject> SpawnListsObjects { get; set; } = new();

        private static List<SpawnSystemList> SpawnLists { get; set; } = new();

        static SpawnSystemInstantiateSpawnListPatch()
        {
            StateResetter.Subscribe(() =>
            {
                HasInstantiatedSpawnLists = false;
                SpawnListsObjects.Clear();
                SpawnLists.Clear();
            });
        }

        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(nameof(SpawnSystem.Awake))]
        [HarmonyPrefix]
        private static void InstantiateAndReplace(SpawnSystem __instance)
        {
            if (HasInstantiatedSpawnLists)
            {
                __instance.m_spawnLists = SpawnLists;

#if FALSE && DEBUG
                Log.LogTrace("Overriding SpawnSystem spawn lists with instantiated:");

                foreach(var spawnList in SpawnLists)
                {
                    if (!spawnList || spawnList is null)
                    {
                        Log.LogTrace("\tSpawnList: null");
                        continue;
                    }

                    Log.LogTrace("\tSpawnList: " + spawnList.name);

                    foreach(var spawner in spawnList.m_spawners)
                    {
                        if (spawner is null || !spawner.m_prefab)
                        {
                            Log.LogTrace("\t\tnull");
                        }
                        else 
                        {
                            Log.LogTrace("\t\t" + spawner.m_name + ":" + spawner.m_prefab.name);
                        }
                    }
                }
#endif
                return;
            }

            try
            {
                foreach (var spawnList in __instance.m_spawnLists)
                {
                    Log.LogTrace($"Instantiating spawn list: '{spawnList.name}'");
                    var instantiatedSpawnList = GameObject.Instantiate(spawnList.gameObject);

                    SpawnListsObjects.Add(instantiatedSpawnList);
                    SpawnLists.Add(instantiatedSpawnList.GetComponent<SpawnSystemList>());
                }
            }
            catch(Exception e)
            {
                Log.LogWarning("Unable to instantiate SpawnSystemLists. Skipping step. Avoid entering multiple worlds without restarting, then everything will still be fine: " + e.Message);
                HasInstantiatedSpawnLists = true;
                return;
            }

            __instance.m_spawnLists = SpawnLists;

            HasInstantiatedSpawnLists = true;
        }
    }
}
