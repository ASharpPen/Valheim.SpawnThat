
namespace Valheim.SpawnThat.Spawners.LocalSpawner.Models;

public record RoomIdentifier
{
    public string Room { get; set; }
    public string PrefabName { get; set; }

    public RoomIdentifier()
    { }

    public RoomIdentifier(string room, string prefabName)
    {
        Room = room;
        PrefabName = prefabName;
    }
}
