using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General;
using Valheim.SpawnThat.Utilities;
using Valheim.SpawnThat.Utilities.Extensions;

namespace Valheim.SpawnThat.Spawns
{
    [HarmonyPatch(typeof(MonsterAI))]
    public static class MonsterAISetDespawnPatch
    {
        private static MethodInfo DespawnInDay = AccessTools.Method(typeof(MonsterAI), "DespawnInDay");
        private static MethodInfo MoveAwayAndDespawn = AccessTools.Method(typeof(BaseAI), "MoveAwayAndDespawn");

        private static MethodInfo CheckIfShouldDespawnMethod = AccessTools.Method(typeof(MonsterAISetDespawnPatch), nameof(CheckIfShouldDespawn));

        [HarmonyPatch("UpdateAI")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> AddDespawnSection(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                // Move forward to inner scope of the if-statement starting the despawn.
                .MatchForward(false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldarg_1),
                    new CodeMatch(x => x.opcode == OpCodes.Ldc_I4_0 || x.opcode == OpCodes.Ldc_I4_1), // Don't want this to break every time someone changes their mind on making creatures run or walk.
                    new CodeMatch(OpCodes.Call, MoveAwayAndDespawn))
                .AddLabel(out Label innerScopeLabel)
                //Move back to before start of if-statement.
                .MatchBack(false,
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Call, DespawnInDay))
                //Move to right after the loading of MonsterAI instance, to ensure other branches won't be skipping our injected if.
                .Advance(1)
                //Insert own if statement, and jump to inner scope if true.
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, CheckIfShouldDespawnMethod))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, innerScopeLabel))
                //Ensure we leave the stack intact for the next statement.
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InstructionEnumeration();
        }

        private static ConditionalWeakTable<MonsterAI, DespawnState> DespawnStateTable = new();

        private static bool CheckIfShouldDespawn(MonsterAI monsterAI)
        {
            var zdo = ComponentCache.GetZdo(monsterAI);

            var despawnState = DespawnStateTable.GetOrCreateValue(monsterAI);

            if(despawnState.ShouldDespawn)
            {
                return true;
            }

            if(despawnState.LastCheck is not null)
            {
                if(DateTimeOffset.UtcNow - despawnState.LastCheck < TimeSpan.FromSeconds(1))
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

        private class DespawnState
        {
            public DateTimeOffset? LastCheck = null;
            public bool ShouldDespawn = false;
        }

        private static bool CheckDespawnOnAlert(MonsterAI monsterAI, ZDO zdo)
        {
            if (!zdo.GetBool(SpawnModifierDespawnOnAlert.ZdoFeature, false))
            {
                return false;
            }

            return monsterAI.IsAlerted();
        }

        private static bool CheckDespawnOnInvalidConditions(MonsterAI monsterAI, ZDO zdo)
        {
            if (!zdo.GetBool(SpawnModifierDespawnOnConditionsInvalid.ZdoFeature, false))
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

                var checkDay = zdo.GetBool(ConditionDaytime.ZdoConditionDay, true);
                var checkNight = zdo.GetBool(ConditionDaytime.ZdoConditionNight, true);

                if (!ConditionDaytime.Instance.IsValid(checkDay, checkNight))
                {
                    Log.LogTrace($"Setting {monsterAI.name} to despawn, due to daytime conditions.");
                    return result = true;
                }

                var checkEnvironments = zdo.GetString(ConditionEnvironments.ZdoCondition, "");

                if (!ConditionEnvironments.Instance.IsValid(checkEnvironments))
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
                    zdo.Set(SpawnModifierRelentless.ZdoFeature, false);
                }
            }
        }
    }
}
