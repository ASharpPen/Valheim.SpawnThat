using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Patches
{
    [HarmonyPatch(typeof(CreatureSpawner))]
    public static class CreatureSpawnerConfigureLevelUpChancePatch
    {
        private static MethodInfo UnityRandom = AccessTools.Method(typeof(UnityEngine.Random), nameof(UnityEngine.Random.Range), new[] { typeof(float), typeof(float) });

        private static MethodInfo Detour = AccessTools.Method(typeof(CreatureSpawnerConfigureLevelUpChancePatch), nameof(ConfigureChanceToLevelUp));

        [HarmonyPatch("Spawn")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> ConfigureLevelUpChance(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                // Move to right after call to random chance for leveling up.
                .MatchForward(true,
                    new CodeMatch(OpCodes.Ldc_R4, 0f),
                    new CodeMatch(OpCodes.Ldc_R4, 100f),
                    new CodeMatch(OpCodes.Call, UnityRandom))
                // Advance to right after default value has been loaded unto stack, and insert own detour for replacing it.
                .Advance(2)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, Detour))
                .InstructionEnumeration();
        }

        private static float ConfigureChanceToLevelUp(float defaultChance, CreatureSpawner spawner)
        {
            var config = CreatureSpawnerConfigCache.Get(spawner)?.Config;

            if (config is not null)
            {
                return config.LevelUpChance.Value;
            }
            else
            {
                return defaultChance;
            }
        }
    }
}
