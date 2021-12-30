using HarmonyLib;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    internal static class SpawnPositionFilterPatch
    {
        [HarmonyPatch("IsSpawnPointGood")]
        [HarmonyPrefix]
        private static bool FilterPosition(SpawnSystem.SpawnData spawn, Vector3 spawnPoint, ref bool __result)
        {
            if (spawn is null)
            {
                return true;
            }

            if(SpawnPositionManager.ShouldFilter(spawn, spawnPoint))
            {
                __result = false;
                return false;
            }
            return true;
        }
    }
}
