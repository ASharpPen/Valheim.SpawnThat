
namespace SpawnThat.Spawners.LocalSpawner.Models;

internal record RoomIdentifier
{
    public string Room { get; internal set; }
    public string PrefabName { get; internal set; }

    internal RoomIdentifier()
    { }

    public RoomIdentifier(string room, string prefabName)
    {
        Room = room;
        PrefabName = prefabName;
    }
}
