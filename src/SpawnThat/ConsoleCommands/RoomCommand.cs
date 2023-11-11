using System;
using System.Linq;
using SpawnThat.Core;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Dungeons;

namespace SpawnThat.ConsoleCommands;

internal sealed class RoomCommand
{
    public const string CommandName = "spawnthat:room";

    internal static void Register()
    {
        new Terminal.ConsoleCommand(
            CommandName,
            "Prints if in a dungeon room and which one",
            (args) => Command(args.Context));
    }

    private static void Command(Terminal context)
    {
        try
        {
            if (Player.m_localPlayer.IsNull())
            {
                context.Print(CommandName, "Cannot execute command due to local player object missing.");
                return;
            }

            var pos = Player.m_localPlayer.transform.position;
            var roomData = RoomManager.GetContainingRoom(pos);

            string roomNameCleaned = roomData?.Name?.Split(new[] { '(' })?.FirstOrDefault();

            context.Print(
                CommandName,
                string.IsNullOrWhiteSpace(roomNameCleaned)
                    ? "Not in a room"
                    : roomNameCleaned);
        }
        catch (Exception e)
        {
            Log.LogError($"Error while attempting to execute terminal command '{CommandName}'.", e);
        }
    }
}
