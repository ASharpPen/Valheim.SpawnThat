using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SpawnThat.Spawners.LocalSpawner.Managers;
using SpawnThat.Utilities;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.LocalSpawner.Patches;

[HarmonyPatch(typeof(CreatureSpawner))]
internal static class CreatureSpawner_LocalSpawner_Workflow_Patch
{
    [HarmonyPatch(nameof(CreatureSpawner.UpdateSpawner))]
    [HarmonyPrefix]
    private static void EnsureSpawnerGetsConfigured(CreatureSpawner __instance)
    {
        LocalSpawnerManager.EnsureSpawnerConfigured(__instance);
    }

    [HarmonyPatch(nameof(CreatureSpawner.UpdateSpawner))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.LowerThanNormal)]
    private static bool DelayUpdate(CreatureSpawner __instance)
    {
        return !LocalSpawnerManager.ShouldDelaySpawnerUpdate(__instance);
    }

    [HarmonyPatch(nameof(CreatureSpawner.UpdateSpawner))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> CheckSpawnConditions(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        var spawn = AccessTools.Method(typeof(CreatureSpawner), nameof(CreatureSpawner.Spawn));

        return new CodeMatcher(instructions, generator)
            // Move to Spawn call
            .MatchForward(false, new CodeMatch(OpCodes.Call, spawn))
            // Add label so we can continue flow.
            .CreateLabel(out var callSpawnLabel)
            // Insert check for valid template conditions, and return if not valid.
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(Transpilers.EmitDelegate(LocalSpawnSessionManager.CheckConditionsValid))
            // Continue original flow if conditions valid
            .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, callSpawnLabel))
            // Clean up parameters on stack and return, if not valid.
            .InsertAndAdvance(OpCodes.Pop)
            .InsertAndAdvance(OpCodes.Ret)
            .InstructionEnumeration();
    }

    [HarmonyPatch(nameof(CreatureSpawner.Spawn))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> GetSpawnReference(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to right after instantiation
            .MatchForward(false, new CodeMatch(OpCodes.Call, ReflectionUtils.InstantiateGameObjectMethod))
            .Advance(1)
            // dup gameobject on stack, and call method
            .InsertAndAdvance(OpCodes.Dup)
            .InsertAndAdvance(Transpilers.EmitDelegate(LocalSpawnSessionManager.GetSpawnReference))
            .InstructionEnumeration();
    }

    [HarmonyPatch(nameof(CreatureSpawner.Spawn))]
    [HarmonyPostfix]
    private static void ModifySpawn(CreatureSpawner __instance) => LocalSpawnSessionManager.ModifySpawn(__instance);
}