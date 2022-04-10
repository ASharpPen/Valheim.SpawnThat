using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Dungeons;

namespace SpawnThat.Options.Identifiers;

internal class IdentifierRoom : ISpawnerIdentifier, ICacheableIdentifier
{
    private long _hash;
    private List<string> _rooms;

    public List<string> Rooms
    {
        get { return _rooms; }
        set { SetHash(_rooms = value); }
    }

    internal IdentifierRoom()
    {
    }

    public IdentifierRoom(IEnumerable<string> roomNames)
    {
        Rooms = roomNames
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => x.Trim())
            .ToList();
    }

    public bool IsValid(IdentificationContext context)
    {
        // Ignore if empty.
        if ((Rooms?.Count ?? 0) == 0)
        {
            return true;
        }


        var room = RoomManager.GetContainingRoom(context.Zdo.GetPosition());

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
