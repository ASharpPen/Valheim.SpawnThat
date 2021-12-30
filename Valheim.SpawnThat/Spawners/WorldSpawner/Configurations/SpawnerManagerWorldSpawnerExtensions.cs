namespace Valheim.SpawnThat.Spawners.WorldSpawner.Configurations;

public static class SpawnerManagerWorldSpawnerExtensions
{
    public static IWorldSpawnBuilder ConfigureWorldSpawner(
        this ISpawnerConfigurationCollection configCollection,
        uint id)
        => configCollection
            .GetOrAddSpawnerConfiguration(new WorldSpawnConfigurationCollection())
            .GetBuilder(id);
}
