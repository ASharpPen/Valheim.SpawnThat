using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner;

namespace Valheim.SpawnThat.ConsoleCommands
{
    [HarmonyPatch(typeof(Console))]
    public static class ConsolePatch
    {
        private static MethodInfo Print = AccessTools.Method(typeof(Console), "AddString", new[] { typeof(string) });

        [HarmonyPatch("InputText")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> InsertCommandIntoHelp(IEnumerable<CodeInstruction> instructions)
        {
            return new CodeMatcher(instructions)
                .MatchForward(true, new CodeMatch(OpCodes.Ldstr, "info - print system info"))
                .Advance(2)
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldstr, "spawnthat room - prints if in a dungeon room and which one"))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, Print))
                .InstructionEnumeration();
        }

        [HarmonyPatch("InputText")]
        [HarmonyPrefix]
        private static bool CheckCommands(Console __instance)
        {
            if(Player.m_localPlayer is null)
            {
                return true;
            }

            var text = __instance.m_input.text;
            var commandPieces = text.Split(new char[] { ' ' });

            if(commandPieces.Length < 2)
            {
                return true;
            }

            if (commandPieces[0].ToUpperInvariant() != "SPAWNTHAT")
            {
                return true;
            }

            if (commandPieces[1].ToUpperInvariant() == "ROOM")
            {
                var pos = Player.m_localPlayer.transform.position;
                var roomData = RoomCache.GetContainingRoom(pos);

                __instance.Print(text);

                string roomNameCleaned = roomData?.Name?.Split(new[] { '(' })?.FirstOrDefault();

                if (string.IsNullOrEmpty(roomNameCleaned))
                {
                    __instance.Print("Not in a room");
                }
                else
                {
                    __instance.Print(roomNameCleaned);
                }

                return false;
            }

            return true;
        }
    }
}
