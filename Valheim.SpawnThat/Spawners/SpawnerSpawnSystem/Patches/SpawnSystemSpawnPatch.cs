using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Patches
{
    [HarmonyPatch(typeof(SpawnSystem))]
    public static class SpawnSystemSpawnPatch
    {
        private static MethodInfo ModifySpawnMethod = AccessTools.Method(typeof(SpawnSystemSpawnPatch), nameof(ModifySpawn));

        [HarmonyPatch("Spawn")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> HookSpawned(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(false, new CodeMatch(OpCodes.Stloc_0))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_1))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, ModifySpawnMethod))
                .InstructionEnumeration();
        }

        private static void ModifySpawn(SpawnSystem spawnSystem, SpawnSystem.SpawnData spawner, GameObject spawn, bool isEventCreature)
        {
            if (isEventCreature)
            {
                return;
            }

            SpawnModificationManager.Instance.ApplyModifiers(spawnSystem, spawn, spawner);
        }
    }
}
