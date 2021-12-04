using BepInEx;
using HarmonyLib;
using System.IO;
using System.Linq;
using UnityEngine;
using Valheim.SpawnThat.Configuration;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Maps;
using Valheim.SpawnThat.Maps.Managers;
using Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner;
using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ConsoleCommands
{
    [HarmonyPatch(typeof(Terminal))]
    internal static class TerminalPatch
    {
        [HarmonyPatch(nameof(Terminal.TryRunCommand))]
        [HarmonyPostfix]
        private static void AddCommandsToHelp(string text)
        {
            if (text == "help")
            {
                Console.instance.Print("spawnthat room - prints if in a dungeon room and which one");
                Console.instance.Print("spawnthat area - prints the area id of the players current location");
                Console.instance.Print("spawnthat arearoll [index] - prints the rolled chance for a template, in area player is currently in");
                Console.instance.Print("spawnthat arearoll [index] [x] [y] - prints the rolled chance for a template, in the area with indicated coordinates");
                Console.instance.Print("spawnthat arearollheatmap [index] - prints a png map of area rolls for a template to disk.");
                Console.instance.Print("spawnthat wheredoesitspawn [index] - prints a png map of areas in which the world spawner template with <index> spawns to disk.");
            }
        }

        [HarmonyPatch(nameof(Terminal.TryRunCommand))]
        [HarmonyPrefix]
        private static bool RunOwnCommand(string text)
        {
            if (Player.m_localPlayer is null)
            {
                return true;
            }

            var commandPieces = text.Split(new char[] { ' ' });

            if (commandPieces.Length < 2)
            {
                return true;
            }

            if (commandPieces[0].ToUpperInvariant() != "SPAWNTHAT")
            {
                return true;
            }
            else if (commandPieces[1].ToUpperInvariant() == "TILT")
            {
                PrintCalculatedTilt();
                return false;
            }
            else if (commandPieces[1].ToUpperInvariant() == "ROOM")
            {
                CommandInRoom();
                return false;
            }
            else if (commandPieces.Length >= 3 && commandPieces[1].ToUpperInvariant() == "AREAROLL")
            {
                if (!int.TryParse(commandPieces[2], out int templateIndex))
                {
                    return true;
                }

                if (commandPieces.Length >= 5)
                {
                    if (int.TryParse(commandPieces[3], out int x))
                    {
                        if (int.TryParse(commandPieces[4], out int y))
                        {
                            CommandAreaTemplateSpawnChance(templateIndex, new Vector3(x, y));
                            return false;
                        }
                    }
                }

                CommandAreaTemplateSpawnChance(templateIndex);
                return false;
            }
            else if (commandPieces.Length >= 3 && commandPieces[1].ToUpperInvariant() == "WHEREDOESITSPAWN")
            {
                if (int.TryParse(commandPieces[2], out int templateIndex))
                {
                    CommandPrintTemplateSpawnAreas(templateIndex);
                    return false;
                }
            }
            else if (commandPieces.Length >= 3 && commandPieces[1].ToUpperInvariant() == "AREAROLLHEATMAP")
            {
                if (int.TryParse(commandPieces[2], out int templateIndex))
                {
                    CommandPrintTemplateAreaHeatMap(templateIndex);
                    return false;
                }
            }
            else if (commandPieces.Length >= 2 && commandPieces[1].ToUpperInvariant() == "ZONE")
            {
                var zone = ZoneSystem.instance.GetZone(Player.m_localPlayer.transform.position);

                Console.instance.Print($"Zone: {zone}");
                return false;
            }
            else if (commandPieces.Length >= 2 && commandPieces[1].ToUpperInvariant() == "AREA")
            {
                var areaId = MapManager.GetAreaId(Player.m_localPlayer.transform.position);

                Console.instance.Print($"Area Id: {areaId}");
                return false;
            }
            return true;
        }

        public static void PrintCalculatedTilt()
        {
            var pos = Player.m_localPlayer.transform.position;
            var angle = WorldData.Tilt(pos);

            Console.instance.Print("Tilt: " + angle);
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
            if (position is null)
            {
                position = Player.m_localPlayer?.transform?.position;

                if (position is null)
                {
                    return;
                }
            }

            var areaId = MapManager.GetAreaId(position.Value);
            var chance = MapManager.GetAreaChance(position.Value, templateIndex);

            Console.instance.Print($"Template '{templateIndex}', in area '{areaId}' rolled chance '{chance * 100}'.");
        }

        public static void CommandPrintTemplateAreaHeatMap(int templateIndex)
        {
            float[][] chanceMap = MapManager.GetTemplateAreaChanceMap(templateIndex);
            int[][] heatmap = new int[chanceMap.Length][];

            for (int x = 0; x < heatmap.Length; ++x)
            {
                heatmap[x] = new int[heatmap.Length];

                for (int y = 0; y < heatmap.Length; ++y)
                {
                    heatmap[x][y] = (int)(chanceMap[x][y] * 255);
                }
            }

            ImageBuilder
                .Init(MapManager.AreaMap)
                .AddHeatZones(heatmap, false)
                .Print("Debug", $"area_roll_{templateIndex}");

            var file = Path.Combine("Debug", $"area_roll_{templateIndex}");
            var filePath = Path.Combine(Paths.BepInExRootPath, $"{file}.png");

            Console.instance.Print("Printing heatmap map of templates area rolls to: " + filePath);
        }

        public static void CommandPrintTemplateSpawnAreas(int templateIndex)
        {
            var spawnMap = MapManager.GetSpawnMap(templateIndex);

            //SpawnSystem config is only expected to have a single first layer, namely "WorldSpawner", so we just grab the first entry.
            var spawnSystemConfigs = ConfigurationManager
                .SpawnSystemConfig?
                .Subsections? //[*]
                .Values?
                .FirstOrDefault();

            string prefabName = "";
            if (spawnSystemConfigs.Subsections.TryGetValue(templateIndex.ToString(), out SpawnConfiguration config))
            {
                prefabName = config.PrefabName.Value;
            }
            else
            {
                Console.instance.Print($"Unable to find config for template '{templateIndex}', skipping print.");
                return;
            }

            ImageBuilder
                .SetGrayscaleBiomes(MapManager.AreaMap)
                .AddHeatZones(spawnMap)
                .Print("Debug", $"spawn_map_{templateIndex}_{prefabName}");

            var file = Path.Combine("Debug", $"spawn_map_{templateIndex}_{prefabName}");
            var filePath = Path.Combine(Paths.BepInExRootPath, $"{file}.png");

            Console.instance.Print("Printing map of spawn areas to: " + filePath);
        }
    }
}
