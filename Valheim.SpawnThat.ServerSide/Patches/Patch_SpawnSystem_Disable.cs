using HarmonyLib;

namespace Valheim.SpawnThat.ServerSide.Patches;

[HarmonyPatch]
internal static class Patch_SpawnSystem_Disable
{
    [HarmonyPatch(typeof(SpawnSystem), nameof(SpawnSystem.UpdateSpawning))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.Last)]
    private static bool NoSpawningWhenSimulated()
    {
        return false;
    }
}
