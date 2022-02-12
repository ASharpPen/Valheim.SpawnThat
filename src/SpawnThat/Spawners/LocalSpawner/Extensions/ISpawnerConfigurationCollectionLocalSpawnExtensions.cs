using SpawnThat.Spawners.LocalSpawner;
using SpawnThat.Spawners.LocalSpawner.Configuration;
using SpawnThat.Spawners.LocalSpawner.Models;

namespace SpawnThat.Spawners;

public static class ISpawnerConfigurationCollectionLocalSpawnExtensions
{
    public static ILocalSpawnBuilder ConfigureLocalSpawnerByName(
        this ISpawnerConfigurationCollection configCollection,
        string spawnerPrefabName) 
        => configCollection
            .GetOrAddSpawnerConfiguration(new LocalSpawnerConfiguration())
            .GetBuilder(new SpawnerNameIdentifier(
                spawnerPrefabName));

    public static ILocalSpawnBuilder ConfigureLocalSpawnerByLocationAndCreature(
        this ISpawnerConfigurationCollection configCollection,
        string locationName,
        string creaturePrefabName) 
        => configCollection
            .GetOrAddSpawnerConfiguration(new LocalSpawnerConfiguration())
            .GetBuilder(new LocationIdentifier(
                locationName, 
                creaturePrefabName));

    public static ILocalSpawnBuilder ConfigureLocalSpawnerByRoomAndCreature(
        this ISpawnerConfigurationCollection configCollection,
        string roomName,
        string creaturePrefabName) 
        => configCollection
            .GetOrAddSpawnerConfiguration(new LocalSpawnerConfiguration())
            .GetBuilder(new RoomIdentifier(
                roomName, 
                creaturePrefabName));
}
