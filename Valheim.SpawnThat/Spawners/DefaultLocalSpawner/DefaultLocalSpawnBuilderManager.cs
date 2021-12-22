using System.Collections.Generic;
using Valheim.SpawnThat.Startup;

namespace Valheim.SpawnThat.Spawners.DefaultLocalSpawner;

internal static class DefaultLocalSpawnBuilderManager
{
    private static Dictionary<string, IDefaultLocalSpawnBuilder> spawnerTemplates { get; set; } = new();
    private static Dictionary<LocationIdentifier, IDefaultLocalSpawnBuilder> locationTemplates { get; set; } = new();
    private static Dictionary<RoomIdentifier, IDefaultLocalSpawnBuilder> roomTemplates { get; set; } = new();

    static DefaultLocalSpawnBuilderManager()
    {
        StateResetter.Subscribe(() =>
        {
            spawnerTemplates = new();
            locationTemplates = new();
            roomTemplates = new();
        });
    }

    public static void RegisterLocationTemplate(string location, string prefabName, IDefaultLocalSpawnBuilder template)
    {
        locationTemplates[new(location, prefabName)] = template;
    }

    public static void RegisterRoomTemplate(string room, string prefabName, IDefaultLocalSpawnBuilder template)
    {
        roomTemplates[new RoomIdentifier(room, prefabName)] = template;
    }

    public static void RegisterSpawnerNameTemplate(string spawnerPrefabName, IDefaultLocalSpawnBuilder template)
    {
        spawnerTemplates[spawnerPrefabName] = template;
    }

    private record struct LocationIdentifier(string Location, string PrefabName);

    private record struct RoomIdentifier(string Room, string PrefabName);
}
