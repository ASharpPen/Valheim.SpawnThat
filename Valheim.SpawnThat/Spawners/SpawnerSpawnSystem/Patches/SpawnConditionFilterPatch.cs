using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class SpawnConditionFilterPatch
    {
        private static FieldInfo FieldAnchor = AccessTools.Field(typeof(SpawnSystem.SpawnData), "m_enabled");
        private static MethodInfo FilterMethod = AccessTools.Method(typeof(SpawnConditionFilterPatch), nameof(FilterSpawners), new[] { typeof(SpawnSystem), typeof(SpawnSystem.SpawnData), typeof(bool) });

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
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Callvirt, FilterMethod))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Brtrue, escapeLoopLabel))
                .InstructionEnumeration();
        }

        private static bool FilterSpawners(SpawnSystem spawner, SpawnSystem.SpawnData spawn, bool eventSpawners)
        {
            if (eventSpawners)
            {
                return false;
            }

            try
            {
                return SpawnConditionManager.Instance.FilterOnSpawn(spawner, spawn);
            }
            catch(Exception e)
            {
                Log.LogError($"Error while checking if spawn template {spawn?.m_prefab?.name} should be filtered.", e);
                return false;
            }
        }
    }
}
