using System;
using System.Collections.Generic;
using HarmonyLib;

namespace SpawnThat.Spawners.DestructibleSpawner.Patches;

[HarmonyPatch(typeof(SpawnArea))]
internal static class SpawnArea_DestructibleSpawner_Workflow_Patch
{
    [HarmonyPatch(nameof(SpawnArea.UpdateSpawn))]
    [HarmonyPrefix]
    private static void ConfigureSpawner()
    {
    }

    [HarmonyPatch(nameof(SpawnArea.UpdateSpawn))]
    [HarmonyPrefix]
    private static void DelayUpdate()
    {
    }

    [HarmonyPatch(nameof(SpawnArea.SelectWeightedPrefab))]
    [HarmonyPrefix]
    private static void FilterSpawnData()
    {
    }

    [HarmonyPatch(nameof(SpawnArea.SelectWeightedPrefab))]
    [HarmonyPostfix]
    private static void ResetSpawnData()
    {
    }

    [HarmonyPatch(nameof(SpawnArea.FindSpawnPoint))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> CheckValidPosition(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .InstructionEnumeration();
    }

    [HarmonyPatch(nameof(SpawnArea.Awake))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ModifySpawn(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .InstructionEnumeration();
    }
}
