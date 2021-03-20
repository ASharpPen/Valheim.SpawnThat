using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Valheim.SpawnThat.ConfigurationCore;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class PreSpawnConditionChecker
    {
        private static FieldInfo FieldAnchor = AccessTools.Field(typeof(SpawnSystem.SpawnData), "m_enabled");
        private static MethodInfo FilterMethod = AccessTools.Method(typeof(PreSpawnConditionChecker), nameof(FilterSpawners), new[] { typeof(SpawnSystem.SpawnData), typeof(bool) });

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> AddFilterConditions(IEnumerable<CodeInstruction> instructions)
        {
            var matcher = new CodeMatcher(instructions)
                .MatchForward(
                    true,
                    new CodeMatch(OpCodes.Ldfld, FieldAnchor),
                    new CodeMatch(OpCodes.Brfalse));

            var escapeLoopLabel = matcher.Operand;

            return matcher
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Callvirt, FilterMethod))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, escapeLoopLabel))
                .InstructionEnumeration();
        }

        private static bool FilterSpawners(SpawnSystem.SpawnData spawner, bool eventSpawners)
        {
            if (eventSpawners)
            {
                return false;
            }

            int day = EnvMan.instance.GetDay(ZNet.instance.GetTimeSeconds());

            List<int> indexesToRemove = new List<int>();

            var cache = SpawnDataCache.Get(spawner);

            if(cache?.Config == null)
            {
                return false;
            }

            if (cache.Config.ConditionWorldAgeDaysMin.Value > 0 && cache.Config.ConditionWorldAgeDaysMin.Value > day)
            {
#if DEBUG
                Log.LogInfo($"Filtering spawner {spawner.m_name} due to world not being old enough.");
#endif
                return true;
            }

            if(cache.Config.ConditionWorldAgeDaysMax.Value > 0 && cache.Config.ConditionWorldAgeDaysMax.Value < day)
            {
#if DEBUG
                Log.LogInfo($"Filtering spawner {spawner.m_name} due to world being too old.");
#endif
                return true;
            }

            return false;
        }
    }
}
