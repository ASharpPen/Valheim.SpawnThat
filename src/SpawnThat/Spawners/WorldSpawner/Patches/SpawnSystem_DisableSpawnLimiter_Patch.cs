using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.WorldSpawner.Patches;

internal static class SpawnSystem_DisableSpawnLimiter_Patch
{
    public static void Patch(Harmony harmony)
    {
        try
        {
            MethodInfo transpilerMethod = ((Func<IEnumerable<CodeInstruction>, IEnumerable<CodeInstruction>>)ToggleSpawnLimiter).Method;

            harmony.Patch(
                original: AccessTools.Method(typeof(SpawnSystem), nameof(SpawnSystem.UpdateSpawnList)),
                transpiler: new HarmonyMethod(transpilerMethod));
        }
        catch (Exception e)
        {
            Log.LogWarning("Failed to apply patch required for WorldSpawner.DisableSpawnLimiter. Feature unavailable.");
        }
    }

    private static IEnumerable<CodeInstruction> ToggleSpawnLimiter(IEnumerable<CodeInstruction> instructions)
    {
        MethodInfo currentSpawnCount = AccessTools.Method(typeof(SpawnSystem), nameof(SpawnSystem.GetNrOfZDOInstances));
        FieldInfo maxSpawned = AccessTools.Field(typeof(SpawnSystem.SpawnData), nameof(SpawnSystem.SpawnData.m_maxSpawned));

        var matcher = new CodeMatcher(instructions)
            // Move to after number of creatures is counted.
            .MatchForward(
                false,
                new CodeMatch(OpCodes.Call, currentSpawnCount))
            .GetPosition(out int spawnCountPos)
            // Move to where the limiter is used
            .MatchForward(
                false,
                new CodeMatch(OpCodes.Ldfld, maxSpawned))
            .HasSpawnLimitMarker(spawnCountPos, out bool hasMarker);

        if (hasMarker)
        {
            matcher = matcher
                .MatchBack(
                    false,
                    new CodeMatch(OpCodes.Add))
                .InsertAndAdvance(Transpilers.EmitDelegate(WorldSpawnerManager.OverrideSpawnLimiter));
        }

        return matcher
            .InstructionEnumeration();
    }

    private static CodeMatcher HasSpawnLimitMarker(this CodeMatcher codeMatcher, int earliestPos, out bool hasMarker)
    {
        hasMarker = false;

        codeMatcher.GetPosition(out int currentPos);

        int minOffset = earliestPos - currentPos;

        for (int offset = 0; offset >= minOffset; --offset)
        {
            if (codeMatcher.InstructionAt(offset).opcode == OpCodes.Add)
            {
                hasMarker = true;
                break;
            }
        }

        return codeMatcher;
    }
}
