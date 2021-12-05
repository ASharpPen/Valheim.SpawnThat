using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace Valheim.SpawnThat.ServerSide.Patches;

[HarmonyPatch]
internal static class Patch_RandEventSystem_FixedUpdate_FixActiveEventOnServer
{
    private static FieldInfo LocalPlayerField = AccessTools.Field(typeof(Player), nameof(Player.m_localPlayer));
    private static MethodInfo IsInsideRandomEventAreaMethod = AccessTools.Method(typeof(RandEventSystem), nameof(RandEventSystem.IsInsideRandomEventArea));
    private static MethodInfo ServerSafeIsInsideEventAreaMethod =
        AccessTools.Method(
            typeof(Patch_RandEventSystem_FixedUpdate_FixActiveEventOnServer),
            nameof(ServerSafeIsInsideEventArea),
            new[] { typeof(RandEventSystem), typeof(RandomEvent)});

    [HarmonyPatch(typeof(RandEventSystem), nameof(RandEventSystem.FixedUpdate))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> RemoveLocalPlayerCheck(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to right before !Player.m_localPlayer check.
            .MatchForward(false, 
                new CodeMatch(OpCodes.Brfalse),
                new CodeMatch(OpCodes.Ldsfld, LocalPlayerField))
            .Advance(1)
            // Remove local player check.
            .RemoveInstructions(3)
            .InstructionEnumeration();
    }

    [HarmonyPatch(typeof(RandEventSystem), nameof(RandEventSystem.FixedUpdate))]
    [HarmonyTranspiler]
    private static IEnumerable<CodeInstruction> ReplacePlayerInsideRaidAreaCheck(IEnumerable<CodeInstruction> instructions)
    {
        return new CodeMatcher(instructions)
            // Move to IsInsideRandomEventArea check.
            .MatchForward(false, new CodeMatch(OpCodes.Call, IsInsideRandomEventAreaMethod))
            // Replace with custom method.
            .SetInstruction(new CodeInstruction(OpCodes.Call, ServerSafeIsInsideEventAreaMethod))
            // Move back to local player being loaded onto stack
            .MatchBack(false, new CodeMatch(OpCodes.Ldsfld, LocalPlayerField))
            // Remove local player references
            .RemoveInstructions(3)
            .InstructionEnumeration();
    }

    private static bool ServerSafeIsInsideEventArea(RandEventSystem randEventSystem, RandomEvent randomEvent)
    {
        if (ZNet.instance.IsServer())
        {
            var players = ZNet.instance.GetAllCharacterZDOS();

            if ((players?.Count ?? 0) == 0)
            {
                return false;
            }

            return players.Any(x => randEventSystem.IsInsideRandomEventArea(randomEvent, x.GetPosition()));
        }
        else
        {
            return randEventSystem.IsInsideRandomEventArea(randomEvent, Player.m_localPlayer.transform.position);
        }
    }
}
