using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.SpawnModifiers;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner.Patches
{
    [HarmonyPatch(typeof(CreatureSpawner))]
    public static class OnSpawnPatch
    {
        private static MethodInfo ModifySpawnMethod = AccessTools.Method(typeof(OnSpawnPatch), nameof(ModifySpawn));

        [HarmonyPatch("Spawn")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> HookSpawned(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Stloc_3))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, ModifySpawnMethod))
                .InstructionEnumeration();
        }

        private static void ModifySpawn(CreatureSpawner spawner, GameObject spawn)
        {
            var config = CreatureSpawnerConfigCache.Get(spawner);

            if(config?.Config is null)
            {
                Log.LogTrace($"Found no config for {spawn}.");
                return;
            }

            Log.LogTrace($"Applying modifiers to spawn {spawn.name}");

            SpawnModificationManager.Instance.ApplyModifiers(spawn, config.Config);
        }
    }
}
