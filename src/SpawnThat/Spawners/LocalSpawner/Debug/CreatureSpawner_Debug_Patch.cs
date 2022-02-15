// #define VERBOSE
#if DEBUG && VERBOSE

using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;

namespace SpawnThat.Spawners.LocalSpawner.Debug;

[HarmonyPatch(typeof(CreatureSpawner))]
internal static class CreatureSpawner_Debug_Patch
{
    private static CreatureSpawner Spawner;

    [HarmonyPatch(nameof(CreatureSpawner.UpdateSpawner))]
    [HarmonyPrefix]
    private static void StartSession(CreatureSpawner __instance)
    {
        LogMessage(__instance, "UpdateSpawner");
        Spawner = __instance;
    }

    [HarmonyPatch(nameof(CreatureSpawner.UpdateSpawner))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> Debug(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to IsOwner check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep1)))
            .Advance(1)
            // Move to is alive check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep2)))
            .Advance(1)
            // Move to has-spawned-but-don't-respawn check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep3)))
            .Advance(1)
            // Move to too-early-for-respawn check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep4)))
            .Advance(1)
            // Move to is-day check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep5)))
            .Advance(1)
            // Move to is-night check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep6)))
            .Advance(1)
            // Move to not-inside-playerbase check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep7)))
            .Advance(1)
            // Move to noise check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep8)))
            .Advance(1)
            // Move to distance check
            .MatchForward(false, new CodeMatch(OpCodes.Ret))
            .InsertAndAdvance(OpCodes.Ldarg_0)
            .InsertAndAdvance(AccessTools.Method(typeof(CreatureSpawner_Debug_Patch), nameof(LogStep9)))
            .Advance(1)
            .InstructionEnumeration();
    }

    private static void LogStep1(CreatureSpawner __instance)
    {
        LogMessage(__instance, "Not owner: " + Spawner.m_nview.IsOwner());
    }

    private static void LogStep2(CreatureSpawner __instance)
    {
        LogMessage(__instance, "Is alive.");
    }

    private static void LogStep3(CreatureSpawner __instance)
    {
        LogMessage(__instance, "Has spawned, but respawn time 0 or less: " + Spawner.m_respawnTimeMinuts + ", " + Spawner.m_nview.GetZDO().GetZDOID("spawn_id"));
    }

    private static void LogStep4(CreatureSpawner __instance)
    {
        var timeSinceRespawn =
            new DateTime(Spawner.m_nview.GetZDO().GetLong("alive_time", 0L))
            -
            ZNet.instance.GetTime();

        LogMessage(__instance, $"Not time for respawn yet. {timeSinceRespawn.Minutes} < {Spawner.m_respawnTimeMinuts}");
    }

    private static void LogStep5(CreatureSpawner __instance)
    {
        LogMessage(__instance, $"Can't spawn during day. Day = " + EnvMan.instance.IsDay());
    }

    private static void LogStep6(CreatureSpawner __instance)
    {
        LogMessage(__instance, $"Can't spawn during night. Night = " + EnvMan.instance.IsNight());
    }

    private static void LogStep7(CreatureSpawner __instance)
    {
        LogMessage(__instance, $"Can't spawn inside player base.");
    }

    private static void LogStep8(CreatureSpawner __instance)
    {
        LogMessage(__instance, $"Not enough noise or distance from player.");
    }

    private static void LogStep9(CreatureSpawner __instance)
    {
        LogMessage(__instance, $"Player not close enough.");
    }

    private static void LogMessage(CreatureSpawner instance, string message)
    {
        Log.LogTrace($"[{instance.name}:{instance.transform.position}]: " + message);
    }
}


#endif