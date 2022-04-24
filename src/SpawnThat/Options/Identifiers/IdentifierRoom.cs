using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SpawnThat.Core;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Dungeons;

namespace SpawnThat.Options.Identifiers;

public class IdentifierRoom : ISpawnerIdentifier, ICacheableIdentifier
{
    private long _hash;
    private ICollection<string> _rooms;

    public ICollection<string> Rooms
    {
        get { return _rooms; }
        set 
        { 
            _rooms = value
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToList();

            SetHash(_rooms); 
        }
    }

    internal IdentifierRoom()
    {
    }

    public IdentifierRoom(IEnumerable<string> roomNames)
    {
        Rooms = roomNames.ToArray();
    }

    public bool IsValid(IdentificationContext context)
    {
        // Ignore if empty.
        if ((Rooms?.Count ?? 0) == 0)
        {
            return true;
        }


        var room = RoomManager.GetContainingRoom(context.Zdo.GetPosition());

#if DEBUG
        Log.LogTrace($"Is room '{room?.Name}' in '{Rooms.Join()}': {Rooms.Any(x => x == room?.Name)}");
#endif

        if (room is null)
        {
            return false;
        }

        var match = Rooms.Any(x => x == room.Name);

        return match;
    }

    private void SetHash(ICollection<string> names)
    {
        _hash = 0;

        foreach (var name in names)
        {
            unchecked
            {
                _hash += name.GetStableHashCode();
            }
        }
    }

    public long GetParameterHash()
    {
        return _hash;
    }

    public int GetMatchWeight() => MatchWeight.High;
}
