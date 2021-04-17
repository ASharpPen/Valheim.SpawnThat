using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class PreSpawnFilterPatch
    {
        private static FieldInfo FieldAnchor = AccessTools.Field(typeof(SpawnSystem.SpawnData), "m_enabled");
        private static MethodInfo FilterMethod = AccessTools.Method(typeof(PreSpawnFilterPatch), nameof(FilterSpawners), new[] { typeof(SpawnSystem.SpawnData), typeof(bool) });

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

            return ConditionManager.Instance.FilterOnSpawn(spawner);
        }
    }
}
