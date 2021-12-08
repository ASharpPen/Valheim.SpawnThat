using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace Valheim.SpawnThat.ServerSide.Patches;

// TODO: What was the purpose of this again? We are simulating everything so...
[HarmonyPatch(typeof(SpawnSystem))]
internal static class Patch_SpawnSystem_UpdateSpawning_RemoveServerRestriction
{
    /// <summary>
    /// Removes the check for local player, that stops server from running SpawnSystem updates.
    /// </summary>
    [HarmonyPatch(nameof(SpawnSystem.UpdateSpawning))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> RemoveServerRestriction(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            .MatchForward(false, new CodeMatch(OpCodes.Ldsfld, AccessTools.Field(typeof(Player), nameof(Player.m_localPlayer))))
            .Advance(1)
            // Pop the current player check.
            .InsertAndAdvance(new CodeInstruction(OpCodes.Pop))
            // Remove remaining part of check.
            .RemoveInstructions(4)
            .InstructionEnumeration();
    }
}
