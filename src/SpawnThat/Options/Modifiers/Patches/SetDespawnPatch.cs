using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Reflection;
using HarmonyLib;
using SpawnThat.Core.Cache;
using SpawnThat.Caches;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;
using SpawnThat.Utilities;

namespace SpawnThat.Options.Modifiers.Patches;

[HarmonyPatch]
internal static class SetDespawnPatch
{
    private static ManagedCache<DespawnState> DespawnStateTable { get; } = new();

    private class DespawnState
    {
        public DateTimeOffset? LastCheck = null;
        public bool ShouldDespawn = false;
    }

    [HarmonyPatch(typeof(MonsterAI), nameof(MonsterAI.UpdateAI))]
    [HarmonyTranspiler]
    public static IEnumerable<CodeInstruction> AddDespawnSection(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
    {
        MethodInfo DespawnInDay = AccessTools.Method(typeof(MonsterAI), nameof(MonsterAI.DespawnInDay));
        MethodInfo MoveAwayAndDespawn = AccessTools.Method(typeof(BaseAI), nameof(BaseAI.MoveAwayAndDespawn));

        return new CodeMatcher(instructions, generator)
            // Move forward to inner scope of the if-statement starting the despawn.
            .MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldarg_1),
                new CodeMatch(x => x.opcode == OpCodes.Ldc_I4_0 || x.opcode == OpCodes.Ldc_I4_1), // Don't want this to break every time someone changes their mind on making creatures run or walk.
                new CodeMatch(OpCodes.Call, MoveAwayAndDespawn))
            .CreateLabel(out Label innerScopeLabel)
            //Move back to before start of if-statement.
            .MatchBack(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Call, DespawnInDay))
            //Move to right after the loading of MonsterAI instance, to ensure other branches won't be skipping our injected if.
            .Advance(1)
            //Insert own if statement, and jump to inner scope if true.
            .InsertAndAdvance(Transpilers.EmitDelegate(CheckIfShouldDespawn))
            .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, innerScopeLabel))
            //Ensure we leave the stack intact for the next statement.
            .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
            .InstructionEnumeration();
    }

    private static bool CheckIfShouldDespawn(MonsterAI monsterAI)
    {
        var zdo = ComponentCache.GetZdo(monsterAI);

        var despawnState = DespawnStateTable.GetOrCreate(monsterAI);

        if (despawnState.ShouldDespawn)
        {
            return true;
        }

        if (despawnState.LastCheck is not null)
        {
            if (DateTimeOffset.UtcNow - despawnState.LastCheck < TimeSpan.FromSeconds(1))
            {
                return false;
            }
        }

        bool shouldDespawn = false;
        try
        {
            if (CheckDespawnOnAlert(monsterAI, zdo))
            {
                Log.LogTrace($"Setting {monsterAI.name} to despawn due to alert status.");
                return shouldDespawn = true;
            }

            if (CheckDespawnOnInvalidConditions(monsterAI, zdo))
            {
                return shouldDespawn = true;
            }
        }
        finally
        {
            despawnState.ShouldDespawn = shouldDespawn;
            despawnState.LastCheck = DateTimeOffset.UtcNow;
        }

        return shouldDespawn;
    }

    private static bool CheckDespawnOnAlert(MonsterAI monsterAI, ZDO zdo)
    {
        if (!zdo.GetBool(ModifierDespawnOnAlert.ZdoFeature, false))
        {
            return false;
        }

        return monsterAI.IsAlerted();
    }

    private static bool CheckDespawnOnInvalidConditions(MonsterAI monsterAI, ZDO zdo)
    {
        if (!zdo.GetBool(ModifierDespawnOnConditionsInvalid.ZdoFeatureHash, false))
        {
            return false;
        }

        bool result = false;
        try
        {
            if (zdo is null)
            {
                Log.LogTrace("MonsterAI zdo is null");
                return result = false;
            }

            var allowDuringDay = zdo.GetBool(ModifierDespawnOnConditionsInvalid.ZdoConditionDayHash, true);
            var allowDuringNight = zdo.GetBool(ModifierDespawnOnConditionsInvalid.ZdoConditionNightHash, true);

            if (!IsValidDaytime(allowDuringDay, allowDuringNight))
            {
                Log.LogTrace($"Setting {monsterAI.name} to despawn, due to daytime conditions.");
                return result = true;
            }

            var allowedEnvironments = zdo.GetString(ModifierDespawnOnConditionsInvalid.ZdoConditionEnvironmentHash, null);

            if (!IsValidEnvironment(allowedEnvironments))
            {
                Log.LogTrace($"Setting {monsterAI.name} to despawn, due to environment conditions.");
                return result = true;
            }

            return result = false;
        }
        finally
        {
            if (result)
            {
                zdo.Set(ModifierSetRelentless.ZdoFeatureHash, false);
            }
        }
    }

    public static bool IsValidDaytime(bool spawnDuringDay, bool spawnDuringNight)
    {
        var envMan = EnvMan.instance;

        if (!spawnDuringDay && envMan.IsDay())
        {
            return false;
        }

        if (!spawnDuringNight && envMan.IsNight())
        {
            return false;
        }

        return true;
    }

    public static bool IsValidEnvironment(string requiredEnvironments)
    {
        if (string.IsNullOrWhiteSpace(requiredEnvironments))
        {
            return true;
        }

        var envMan = EnvMan.instance;
        var currentEnv = envMan.GetCurrentEnvironment()?.m_name?.Trim()?.ToUpperInvariant();

        var environments = requiredEnvironments.SplitByComma(true);

        foreach (var requiredEnvironment in environments)
        {
            if (string.IsNullOrWhiteSpace(requiredEnvironment))
            {
                continue;
            }

            if (requiredEnvironment == currentEnv)
            {
                return true;
            }
        }

        return false;
    }
}
