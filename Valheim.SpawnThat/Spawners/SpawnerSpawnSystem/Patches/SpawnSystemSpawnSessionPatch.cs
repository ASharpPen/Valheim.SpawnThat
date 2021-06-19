using HarmonyLib;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    internal static class SpawnSystemSpawnSessionPatch
    {
        [HarmonyPatch(nameof(SpawnSystem.UpdateSpawnList))]
        [HarmonyPrefix]
        private static void InitSpawnSession(SpawnSystem __instance)
        {
            SpawnSessionManager.ResetSession();

#if DEBUG
            Log.LogDebug($"Starting new spawn session at {__instance.transform.position}");
#endif
            // 250 is a rough guess, but it seems about right for the currently loaded area distance.
            SpawnSessionManager.Instance.SetService(new SpawnCounter(__instance.transform.position, 250));
        }

        [HarmonyPatch(nameof(SpawnSystem.UpdateSpawnList))]
        [HarmonyPostfix]
        private static void EndSpawnSession()
        {
            SpawnSessionManager.ResetSession();
        }
    }
}
