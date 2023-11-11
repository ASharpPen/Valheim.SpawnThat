using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using SpawnThat.Configuration;
using SpawnThat.Core;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Spawners.WorldSpawner.Services;
using SpawnThat.World.Maps.Area;
using SpawnThat.World.Maps;

namespace SpawnThat.ConsoleCommands;

internal sealed class WhereDoesItSpawnCommand
{
    public const string CommandName = "spawnthat:wheredoesitspawn";

    internal static void Register()
    {
        new Terminal.ConsoleCommand(
            CommandName,
            "[spawn id] - Prints a png map to disk of areas in which the world spawner template with [spawn id] spawns.",
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
                    "Must specify a world spawn id used in Spawn That files.");
                return;
            }

            if (!int.TryParse(args[1], out int templateIndex))
            {
                terminal.Print(
                    CommandName,
                    "Unknown id. Must be a number, eg., 1, 5322 or 1000123.");
                return;
            }

            var spawnMap = WorldSpawnerSpawnMapService.GetMapOfTemplatesActiveAreas(templateIndex);

            var template = WorldSpawnTemplateManager.GetTemplate(templateIndex);

            if (spawnMap is null || template is null)
            {
                terminal.Print(
                    CommandName,
                    $"Unable to find world spawn '{templateIndex}'. Must use a world spawn id used in Spawn That files");
                return;
            }

            ImageBuilder
               .SetGrayscaleBiomes(MapManager.AreaMap)
               .AddHeatZones(spawnMap)
               .Print($"spawn_map_{templateIndex}_{template.PrefabName ?? template.TemplateName ?? string.Empty}");

            var debugFolder = Path.Combine(Paths.BepInExRootPath, ConfigurationManager.GeneralConfig?.DebugFileFolder?.Value ?? "Debug");

            terminal.Print(
                CommandName,
                "Printing map of spawn areas to: " + debugFolder);
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
