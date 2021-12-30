#if FALSE && DEBUG

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawns.Caches;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Debug
{
    [HarmonyPatch(typeof(SpawnSystem))]
    internal static class Patch_SpawnSystem_InjectLogs
    {
        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> IsEnabledInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(SpawnSystem.SpawnData), "m_enabled")))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(LogIsEnabled))))
                .InstructionEnumeration();
        }

        public static bool LogIsEnabled(bool isEnabled, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Is enabled: {isEnabled}");

            return isEnabled;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> HaveBiomeInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Heightmap), "HaveBiome")))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(LogHaveBiome))))
                .InstructionEnumeration();
        }

        public static bool LogHaveBiome(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Have Biome: {result}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> ToSpawnInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Mathf), "Min", new[] { typeof(int), typeof(int) })))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(ToSpawnLog))))
                .InstructionEnumeration();
        }

        public static int ToSpawnLog(int result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Min(MaxSpawned, (TotalSeconds / SpawnInterval)): {result}");

            if (result is 0)
            {
                Log.LogTrace($"[{spawnData.m_prefab.name}] MaxSpawned: {spawnData.m_maxSpawned}, SpawnInterval: {spawnData.m_spawnInterval}");
            }

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SpawnChanceInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(UnityEngine.Random), "Range", new[] { typeof(float), typeof(float) })))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(LogSpawnChance))))
                .InstructionEnumeration();
        }

        public static float LogSpawnChance(float result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Spawn Roll: {result}, Spawn Chance: {spawnData.m_spawnChance}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> HaveGlobalKeyInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Ldfld, AccessTools.Method(typeof(ZoneSystem), nameof(ZoneSystem.GetGlobalKey))))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(HaveGlobalKeyLog))))
                .InstructionEnumeration();
        }

        public static bool HaveGlobalKeyLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Have Global Key: {result}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> HaveEnvironmentInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(EnvMan), nameof(EnvMan.IsEnvironment), new[] { typeof(List<string>) })))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(HaveEnvironmentLog))))
                .InstructionEnumeration();
        }

        public static bool HaveEnvironmentLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Is Environment: {result}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SpawnAtDayInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(SpawnSystem.SpawnData), "m_spawnAtDay")))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(SpawnAtDayLog))))
                .InstructionEnumeration();
        }

        public static bool SpawnAtDayLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Is Environment: {result}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> SpawnAtNightInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Ldfld, AccessTools.Field(typeof(SpawnSystem.SpawnData), "m_spawnAtNight")))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(SpawnAtNightLog))))
                .InstructionEnumeration();
        }

        public static bool SpawnAtNightLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Is Environment: {result}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> MaxSpawnedInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(SpawnSystem), "GetNrOfInstances", new[] { typeof(GameObject), typeof(Vector3), typeof(float), typeof(bool), typeof(bool) })))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(MaxSpawnedLog))))
                .InstructionEnumeration();
        }

        public static bool MaxSpawnedLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Spawned: {result}, MaxSpawned: {spawnData.m_maxSpawned}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> FindBaseSpawnPointInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(SpawnSystem), "FindBaseSpawnPoint")))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(FindBaseSpawnPointLog))))
                .InstructionEnumeration();
        }

        public static bool FindBaseSpawnPointLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Found base spawn point: {result}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> HaveInstanceInRangeInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(SpawnSystem), "HaveInstanceInRange")))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(HaveInstanceInRangeLog))))
                .InstructionEnumeration();
        }

        public static bool HaveInstanceInRangeLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Have instance in range: {result}");

            return result;
        }

        [HarmonyPatch("UpdateSpawnList")]
        [HarmonyTranspiler]
        public static IEnumerable<CodeInstruction> IsSpawnPointGoodInject(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(SpawnSystem), "IsSpawnPointGood")))
                .Advance(1)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldloc_3))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Patch_SpawnSystem_InjectLogs), nameof(isSpawnPointGoodLog))))
                .InstructionEnumeration();
        }

        public static bool isSpawnPointGoodLog(bool result, SpawnSystem.SpawnData spawnData)
        {
            Log.LogTrace($"[{spawnData.m_prefab.name}] Is spawn point good: {result}");

            return result;
        }
    }
}
#endif