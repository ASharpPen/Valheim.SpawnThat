using System;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Maps;

namespace SpawnThat.ConsoleCommands;

internal sealed class AreaCommand
{
    public const string CommandName = "spawnthat:area";

    public static void Register()
    {
        new Terminal.ConsoleCommand(
            CommandName,
            "Prints the area id of the players current location",
            (args) => Command(args.Context));
    }

    private static void Command(Terminal terminal)
    {
        try
        {
            if (Player.m_localPlayer.IsNull())
            {
                terminal.Print(CommandName, "Cannot execute command due to local player object missing.");
                return;
            }

            var areaId = MapManager.GetAreaId(Player.m_localPlayer.transform.position);

            terminal.Print(CommandName, areaId.ToString());
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to execute terminal command '{CommandName}'.", e);
        }
    }
}
