
using SpawnThat.Core;

namespace SpawnThat.Spawners.LocalSpawner.Models;

internal record RoomIdentifier
{
    public string Room { get; internal set; }
    public string PrefabName { get; internal set; }

    public RoomIdentifier()
    { }

    public RoomIdentifier(string room, string prefabName)
    {
#if DEBUG
        if (string.IsNullOrWhiteSpace(room))
        {
            Log.LogWarning("LocalSpawner builder with empty room for RoomIdentifier detected.");
        }
        if (string.IsNullOrWhiteSpace(prefabName))
        {
            Log.LogWarning("LocalSpawner builder with empty prefabName for RoomIdentifier detected.");
        }
#endif
        Room = room.Trim().ToUpperInvariant();
        PrefabName = prefabName.Trim().ToUpperInvariant();
    }
}
