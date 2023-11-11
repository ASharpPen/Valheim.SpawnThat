using System;
using System.Collections.Generic;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Spawners.WorldSpawner.Managers;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Maps;
using UnityEngine;

namespace SpawnThat.ConsoleCommands;

internal sealed class AreaRollCommand
{
    public const string CommandName = "spawnthat:arearoll";

    internal static void Register()
    {
        new Terminal.ConsoleCommand(
            CommandName,
            "[spawn id] [x] [y] - Prints the areas rolled chance for a world spawn, in area player is currently in or at given coordinates ([x] [y] is optional)",
            (args) => Command(args.Context, args.Args),
            optionsFetcher: CommandOptions);
    }

    private static void Command(Terminal terminal, string[] args)
    {
        try
        {
            if (Player.m_localPlayer.IsNull())
            {
                terminal.Print(CommandName, "Cannot execute command due to local player object missing.");
                return;
            }

            if (args.Length <= 2)
            {
                terminal.Print(CommandName, "Must specify a world spawn id.");
                return;
            }

            if (!int.TryParse(args[1], out int templateIndex))
            {
                terminal.Print(
                    CommandName,
                    "Unknown id. Must be a number, eg., 1, 5322 or 1000123.");
                return;
            }

            // Check for coordinate specifications
            if (args.Length >= 4)
            {
                if (int.TryParse(args[2], out int x))
                {
                    if (int.TryParse(args[3], out int y))
                    {
                        terminal.Print(
                            CommandName,
                            GetAreaTemplateSpawnChance(templateIndex, new Vector3(x, y)));
                        return;
                    }
                }
            }

            terminal.Print(
                CommandName,
                GetAreaTemplateSpawnChance(templateIndex));
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to execute terminal command '{CommandName}'.", e);
        }
    }

    private static string GetAreaTemplateSpawnChance(int templateIndex, Vector3? position = null)
    {
        if (position is null)
        {
            if (Player.m_localPlayer.IsNull())
            {
                return "Player object missing and no position specified.";
            }

            position = Player.m_localPlayer.transform.position;
        }

        var areaId = MapManager.GetAreaId(position.Value);
        var chance = MapManager.GetAreaChance(position.Value, templateIndex);

        return $"World spawn '{templateIndex}', in area '{areaId}', rolled chance '{chance * 100}'.";
    }

    private static List<string> CommandOptions() =>
        WorldSpawnTemplateManager
            .TemplatesById
            .Keys
            .Select(x => x.ToString())
            .ToList();
}
