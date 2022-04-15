using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Spawners.DestructibleSpawner.Managers;
using SpawnThat.Utilities;
using SpawnThat.Utilities.Extensions;
using static SpawnArea;

namespace SpawnThat.Spawners.DestructibleSpawner.Patches;

[HarmonyPatch(typeof(SpawnArea))]
internal static class SpawnArea_DestructibleSpawner_Workflow_Patch
{
    [HarmonyPatch(nameof(SpawnArea.UpdateSpawn))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.Last)]
    private static void ConfigureSpawner(SpawnArea __instance)
    {
        try
        {
            DestructibleSpawnerManager.ConfigureSpawner(__instance);
        }
        catch(Exception e)
        {
            Log.LogError($"Error while attempting to configure destructible spawner '{__instance.GetCleanedName()}'.", e);
        }

        try
        {
            DestructibleSpawnSessionManager.StartSession(__instance);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to init spawn session for destructible spawner '{__instance.GetCleanedName()}'.", e);
        }
    }

    [HarmonyPatch(nameof(SpawnArea.UpdateSpawn))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.LowerThanNormal)]  // Let other prefixes do what they want first.
    private static bool DelayUpdate()
    {
        return !DestructibleSpawnerManager.DelaySpawners;
    }

    [HarmonyPatch(nameof(SpawnArea.SelectWeightedPrefab))]
    [HarmonyPrefix]
    private static void FilterSpawnData(SpawnArea __instance)
    {
        try
        {
            DestructibleSpawnSessionManager.FilterSpawnData(__instance);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while selecting available spawns for destructibe spawner '{__instance.GetCleanedName()}'.", e);
        }
    }

    [HarmonyPatch(nameof(SpawnArea.SelectWeightedPrefab))]
    [HarmonyPostfix]
    private static void GetSelectedPrefab(SpawnArea __instance, SpawnData __result)
    {
        try
        {
            DestructibleSpawnSessionManager.SetCurrentSpawn(__instance, __result);
        }
        catch (Exception e)
        {
            Log.LogWarning($"Error while caching spawn '{__result?.m_prefab.GetCleanedName()}' of destructible spawner '{__instance.GetCleanedName()}'.", e);
        }
    }

    [HarmonyPatch(nameof(SpawnArea.FindSpawnPoint))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> CheckValidPosition(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to right after first original conditional check.
            .MatchForward(false, new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(ZoneSystem), nameof(ZoneSystem.FindFloor))))
            .Advance(1)
            // Get position that original instructions move to if conditions fail.
            .GetOperand(out var falseTarget)
            //.Print(20, 20)
            // Move back before original if's start
            .Start()
            .MatchForward(false, new CodeMatch(OpCodes.Call, AccessTools.PropertyGetter(typeof(ZoneSystem), nameof(ZoneSystem.instance))))
            // Grab the stored position
            .Advance(-1)
            .GetInstruction(out var spawnPoint)
            .Advance(1)
            // Insert custom positional condition checks
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(spawnPoint.GetLdlocFromStLoc())
            .InsertAndAdvance(Transpilers.EmitDelegate(DestructibleSpawnSessionManager.CheckValidPosition))
            .InstructionEnumeration();
    }

    [HarmonyPatch(nameof(SpawnArea.Awake))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ModifySpawn(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to right after instantiation
            .MatchForward(false, new CodeMatch(OpCodes.Call, ReflectionUtils.InstantiateGameObjectMethod))
            .Advance(1)
            // dup item on stack, and call modifier
            .InsertAndAdvance(OpCodes.Dup)
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(Transpilers.EmitDelegate(DestructibleSpawnSessionManager.ModifySpawn))
            .Print(20, 20)
            .InstructionEnumeration();
    }

    [HarmonyPatch(nameof(SpawnArea.UpdateSpawn))]
    [HarmonyFinalizer]
    private static void EndSession(SpawnArea __instance)
    {
        try
        {
            DestructibleSpawnSessionManager.EndSession(__instance);
        }
        catch(Exception e)
        {
            Log.LogError($"Error while attempting to end spawn session for destructible spawner '{__instance.GetCleanedName()}'.", e);
        }
    }
}
