using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
using Valheim.SpawnThat.Caches;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Spawners.WorldSpawner.Patches;

[HarmonyPatch(typeof(SpawnSystem))]
internal static class SpawnSystem_WorldSpawner_Workflow_Patch
{
    [HarmonyPatch(nameof(SpawnSystem.UpdateSpawning))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.Last)]
    private static void InitSpawner(SpawnSystem __instance)
    {
        try
        {
            WorldSpawnerManager.ConfigureSpawnList(__instance);
            WorldSpawnSessionManager.StartSession(__instance);
        }
        catch (Exception e)
        {
            Log.LogError("Error during Spawn That world spawner init", e);
        }
    }

    [HarmonyPatch(nameof(SpawnSystem.UpdateSpawnList))]
    [HarmonyPrefix]
    [HarmonyPriority(Priority.LowerThanNormal)]  // Let other prefixes do what they want first.
    private static bool DelayUpdate(SpawnSystem __instance, bool eventSpawners)
    {
        // Ignore event spawners. Let them do what they do.
        if (eventSpawners)
        {
            return true;
        }

        return !WorldSpawnerManager.ShouldDelaySpawnerUpdate(__instance);
    }

    [HarmonyPatch(nameof(SpawnSystem.UpdateSpawnList))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> CheckSpawnConditions(IEnumerable<CodeInstruction> instructions)
    {
        FieldInfo m_enabled = AccessTools.Field(typeof(SpawnSystem.SpawnData), nameof(SpawnSystem.SpawnData.m_enabled));

        return new CodeMatcher(instructions)
            // Move to right before m_enabled is checked.
            .MatchForward(
                true,
                new CodeMatch(OpCodes.Ldfld, m_enabled),
                new CodeMatch(OpCodes.Brfalse))
            // Get the label to where the code goes if m_enabled == false.
            .GetOperand(out object escapeLoopLabel)
            // Step forward, so we are just past the check.
            .Advance(1)
            // Run spawn session init
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
            .InsertAndAdvance(Transpilers.EmitDelegate(WorldSpawnSessionManager.StartSpawnSession))
            // Run custom conditions for spawn.
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
            .InsertAndAdvance(Transpilers.EmitDelegate(WorldSpawnSessionManager.ValidSpawnEntry))
            .InsertAndAdvance(new CodeInstruction(OpCodes.Brfalse, escapeLoopLabel))
            .InstructionEnumeration();
    }

    [HarmonyPatch(nameof(SpawnSystem.HaveInstanceInRange))]
    [HarmonyPrefix]
    private static bool FixSpawnsInRangeForNonAI(GameObject prefab, Vector3 centerPoint, float minDistance, ref bool __result)
    {
        //Fish and birds all seem to have this tag assigned. It will be counted by the standard method.
        if (prefab.tag == "spawned")
        {
            return true;
        }

        BaseAI baseAI = ComponentCache.Get<BaseAI>(prefab);

        if (baseAI && baseAI is not null)
        {
            return true;
        }

        __result = WorldSpawnSessionManager.AnyInRange(prefab, centerPoint, (int)minDistance);

        return false;
    }

    [HarmonyPatch(nameof(SpawnSystem.GetNrOfInstances), new[] { typeof(GameObject), typeof(Vector3), typeof(float), typeof(bool), typeof(bool) })]
    [HarmonyPrefix]
    private static bool FixSpawnCount(GameObject prefab, Vector3 center, float maxRange, ref int __result)
    {
        //Fish and birds all seem to have this tag assigned. It will be counted by the standard method.
        if (prefab.tag == "spawned")
        {
            return true;
        }

        BaseAI baseAI = ComponentCache.Get<BaseAI>(prefab);

        if (baseAI && baseAI is not null)
        {
            return true;
        }

        __result = WorldSpawnSessionManager.CountEntitiesInArea(prefab, center, maxRange);
        return true;
    }

    [HarmonyPatch(nameof(SpawnSystem.IsSpawnPointGood))]
    [HarmonyPrefix]
    private static bool CheckValidPositionConditions(SpawnSystem.SpawnData spawn, Vector3 spawnPoint, ref bool __result)
    {
        if (spawn is null)
        {
            return true;
        }

        if (!WorldSpawnSessionManager.ValidSpawnPosition(spawnPoint))
        {
            // Early stop. Return immediately and don't try vanilla positional requirements.
            __result = false;
            return false;
        }

        return true;
    }

    [HarmonyPatch(nameof(SpawnSystem.Spawn))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ModifySpawn(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false, new CodeMatch(OpCodes.Stloc_0))
            .Advance(1)
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0))
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
            .InsertAndAdvance(Transpilers.EmitDelegate(WorldSpawnSessionManager.ModifySpawn))
            .InstructionEnumeration();
    }
}
