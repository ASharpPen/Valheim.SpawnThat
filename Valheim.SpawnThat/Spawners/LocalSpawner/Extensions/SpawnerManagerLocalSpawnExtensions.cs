using Valheim.SpawnThat.Spawners.LocalSpawner.Configuration;

namespace Valheim.SpawnThat.Spawners;

public static class SpawnerManagerLocalSpawnExtensions
{
    public static ILocalSpawnBuilder ConfigureLocalSpawnerByName(
        this ISpawnerConfigurationCollection configCollection,
        string spawnerPrefabName) 
        => configCollection
            .GetOrAddSpawnerConfiguration(new LocalSpawnerConfiguration())
            .GetBuilder(new SpawnerNameIdentifier(
                spawnerPrefabName.ToUpperInvariant().Trim()));

    public static ILocalSpawnBuilder ConfigureLocalSpawnerByLocationAndCreature(
        this ISpawnerConfigurationCollection configCollection,
        string locationName,
        string creaturePrefabName) 
        => configCollection
            .GetOrAddSpawnerConfiguration(new LocalSpawnerConfiguration())
            .GetBuilder(new LocationIdentifier(
                locationName.ToUpperInvariant().Trim(), 
                creaturePrefabName.ToUpperInvariant().Trim()));

    public static ILocalSpawnBuilder ConfigureLocalSpawnerByRoomAndCreature(
        this ISpawnerConfigurationCollection configCollection,
        string roomName,
        string creaturePrefabName) 
        => configCollection
            .GetOrAddSpawnerConfiguration(new LocalSpawnerConfiguration())
            .GetBuilder(new RoomIdentifier(
                roomName.ToUpperInvariant().Trim(), 
                creaturePrefabName.ToUpperInvariant().Trim()));
}
