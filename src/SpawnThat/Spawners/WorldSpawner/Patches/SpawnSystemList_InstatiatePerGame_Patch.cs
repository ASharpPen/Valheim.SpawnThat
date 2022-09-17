using HarmonyLib;
using SpawnThat.Spawners.WorldSpawner.Managers;

namespace SpawnThat.Spawners.WorldSpawner.Patches;

/// <summary>
/// All SpawnSystem's reference the raw prefab containing spawn templates.
/// This means any change to the list in that prefab, will carry over between
/// worlds, until game is restarted.
/// 
/// This patch will instantiate the prefab on game start, and then replace
/// the reference to raw, with a reference to the instantiated list.
/// This should result in a conflict-free fix for SpawnSystem modifications,
/// while avoiding the prefab itself being modified.
/// </summary>
[HarmonyPatch]
internal static class SpawnSystemList_InstatiatePerGame_Patch
{
    /// <summary>
    /// Grab the prefab spawn lists as soon as possible.
    /// Intended to reduce later issues with re-instantiating custom spawn lists.
    /// </summary>
    [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.Awake))]
    [HarmonyPostfix]
    private static void PrefabCacher()
    {
        WorldSpawnerManager.SetPrefabSpawnSystemLists();
    }

    [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.Awake))]
    [HarmonyPrefix]
    [HarmonyPriority(int.MaxValue)]
    private static void SetInstantiated(SpawnSystem __instance)
    {
        WorldSpawnerManager.EnsureInstantiatedSpawnListAssigned(__instance);
    }
}
