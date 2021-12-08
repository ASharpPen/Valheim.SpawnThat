using HarmonyLib;

namespace Valheim.SpawnThat.ServerSide.Patches;

[HarmonyPatch]
internal static class Patch_CreatureSpawner_Disable
{
    [HarmonyPatch(typeof(CreatureSpawner), nameof(CreatureSpawner.UpdateSpawner))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.Last)]
    private static bool NoSpawningWhenSimulated()
    {
        return false;
    }
}
