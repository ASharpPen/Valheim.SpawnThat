using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using HarmonyLib;
using SpawnThat.Spawners.LocalSpawner.Managers;

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
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
            .InsertAndAdvance(Transpilers.EmitDelegate(LocalSpawnSessionManager.CheckConditionsValid))
            // Continue original flow if conditions valid
            .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, callSpawnLabel))
            // Clean up parameters on stack and return, if not valid.
            .InsertAndAdvance(new CodeInstruction(OpCodes.Pop))
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ret))
            .InstructionEnumeration();
    }

    [HarmonyPatch(nameof(CreatureSpawner.Spawn))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ConfigureLevelUpChance(IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo UnityRandom = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), new[] { typeof(float), typeof(float) });
        
        return new CodeMatcher(instructions)
            // Move to right after call to random chance for leveling up.
            .MatchForward(true,
                new CodeMatch(OpCodes.Ldc_R4, 0f),
                new CodeMatch(OpCodes.Ldc_R4, 100f),
                new CodeMatch(OpCodes.Call, UnityRandom))
            // Advance to right after default value has been loaded unto stack, and insert own detour for replacing it.
            .Advance(2)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
            .InsertAndAdvance(Transpilers.EmitDelegate(LocalSpawnSessionManager.GetChanceToLevelUp))
            .InstructionEnumeration();
    }

    // TODO: Find more stable anchor. Eg., use the gameobject instantiation call.
    // TODO: Actually, Spawn return the object. Maybe just grab it with a postfix?!
    [HarmonyPatch(nameof(CreatureSpawner.Spawn))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ModifySpawn(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false, new CodeMatch(OpCodes.Stloc_3))
            .Advance(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
            .InsertAndAdvance(Transpilers.EmitDelegate(LocalSpawnSessionManager.ModifySpawn))
            .InstructionEnumeration();
    }
}