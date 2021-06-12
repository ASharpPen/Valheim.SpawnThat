using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner;
using Valheim.SpawnThat.WorldMap;

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
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldstr, "spawnthat TemplateAreaRoll <index> - prints the rolled chance for a template, in area player is currently in"))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Call, Print))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldarg_0))
                .InsertAndAdvance(new CodeInstruction(OpCodes.Ldstr, "spawnthat TemplateAreaRoll <index> <x> <y> - prints the rolled chance for a template, in the area with indicated coordinates"))
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
                Console.instance.Print(text);
                CommandInRoom();
            }
            else if (commandPieces.Length >= 3 && commandPieces[1].ToUpperInvariant() == "TEMPLATEAREAROLL")
            {
                if(!int.TryParse(commandPieces[2], out int templateIndex))
                {
                    return true;
                }

                if(commandPieces.Length >= 5)
                {
                    if(int.TryParse(commandPieces[3], out int x))
                    {
                        if(int.TryParse(commandPieces[4], out int y))
                        {
                            Console.instance.Print(text);
                            CommandAreaTemplateSpawnChance(templateIndex, new Vector3(x, y));

                            return true;
                        }
                    }
                }

                Console.instance.Print(text);
                CommandAreaTemplateSpawnChance(templateIndex);
            }

            return true;
        }

        public static void CommandInRoom()
        {
            var pos = Player.m_localPlayer.transform.position;
            var roomData = RoomCache.GetContainingRoom(pos);

            string roomNameCleaned = roomData?.Name?.Split(new[] { '(' })?.FirstOrDefault();

            if (string.IsNullOrEmpty(roomNameCleaned))
            {
                Console.instance.Print("Not in a room");
            }
            else
            {
                Console.instance.Print(roomNameCleaned);
            }
        }

        public static void CommandAreaTemplateSpawnChance(int templateIndex, Vector3? position = null)
        {
            if(position is null)
            {
                position = Player.m_localPlayer?.transform?.position;

                if(position is null)
                {
                    return;
                }
            }

            var areaId = MapManager.GetAreaId(position.Value);
            var chance = MapManager.GetAreaChance(position.Value, templateIndex);

            Console.instance.Print($"Template '{templateIndex}', in area '{areaId}' rolled chance '{chance}'.");
        }

        public static void CommandPrintAreaTemplateHeatMap(int templateIndex)
        {

        }
    }
}
