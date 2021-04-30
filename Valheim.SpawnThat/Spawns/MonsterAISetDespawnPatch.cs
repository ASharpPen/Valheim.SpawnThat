using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.SpawnModifiers.General;
using Valheim.SpawnThat.Spawns.Caches;
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
                    new CodeMatch(OpCodes.Ldc_I4_1),
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
            var zdo = SpawnCache.GetZDO(monsterAI.gameObject);

            if(!zdo.GetBool(SpawnModifierDespawnOnConditionsInvalid.ZdoFeature, false))
            {
                return false;
            }

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

#if DEBUG
            Log.LogTrace("Checking for despawn.");
#endif

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
#if DEBUG
                    Log.LogTrace($"Setting {monsterAI.name} to despawn, due to daytime conditions.");
#endif
                    return result = true;
                }
                else
                {
#if DEBUG
                    Log.LogTrace($"Daytime conditions for {monsterAI.name} where stil valid");
#endif
                }

                var checkEnvironments = zdo.GetString(ConditionEnvironments.ZdoCondition, "");

                if (!ConditionEnvironments.Instance.IsValid(checkEnvironments))
                {
#if DEBUG
                    Log.LogTrace($"Setting {monsterAI.name} to despawn, due to environment conditions.");
#endif
                    return result = true;
                }
                else
                {
#if DEBUG
                    Log.LogTrace($"Environment conditions '{checkEnvironments}' for {monsterAI.name} where stil valid");
#endif
                }

                return result = false;
            }
            finally
            {
                if(result)
                {
                    zdo.Set(SpawnModifierRelentless.ZdoFeature, false);
                }

                despawnState.LastCheck = DateTimeOffset.UtcNow;
                despawnState.ShouldDespawn = result;
            }
        }

        private class DespawnState
        {
            public DateTimeOffset? LastCheck = null;
            public bool ShouldDespawn = false;
        }
    }
}
