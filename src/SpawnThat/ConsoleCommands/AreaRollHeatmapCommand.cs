using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using SpawnThat.Configuration;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.World.Maps.Area;
using SpawnThat.World.Maps;
using SpawnThat.Core;

namespace SpawnThat.ConsoleCommands;

internal sealed class AreaRollHeatmapCommand
{
    public const string CommandName = "spawnthat:arearollheatmap";

    public static void Register()
    {
        new Terminal.ConsoleCommand(
            CommandName,
            "[spawn id] - Prints a png map of area rolls to disk for a world spawn configured in Spawn That files.",
            (args) => Command(args.Context, args.Args),
            optionsFetcher: CommandOptions);
    }

    private static void Command(Terminal terminal, string[] args)
    {
        try
        {
            if (args.Length <= 2)
            {
                terminal.Print(
                    CommandName,
                    "Must specify a world spawn id used in Spawn That files");
                return;
            }

            if (!int.TryParse(args[1], out int templateIndex))
            {
                terminal.Print(
                    CommandName,
                    "Unknown id. Must be a number, eg., 1, 5322 or 1000123.");
                return;
            }

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
                .Print($"area_roll_{templateIndex}");

            var debugFolder = Path.Combine(Paths.BepInExRootPath, ConfigurationManager.GeneralConfig?.DebugFileFolder?.Value ?? "Debug");

            terminal.Print(
                CommandName, 
                "Printing heatmap map of templates area rolls to: " + debugFolder);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to execute terminal command '{CommandName}'.", e);
        }
    }

    private static List<string> CommandOptions() =>
        WorldSpawnTemplateManager
            .TemplatesById
            .Keys
            .Select(x => x.ToString())
            .ToList();
}
