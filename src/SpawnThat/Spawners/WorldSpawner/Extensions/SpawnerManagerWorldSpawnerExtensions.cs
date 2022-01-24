using SpawnThat.Spawners.WorldSpawner;
using SpawnThat.Spawners.WorldSpawner.Configurations;

namespace SpawnThat.Spawners;

public static class SpawnerManagerWorldSpawnerExtensions
{
    public static IWorldSpawnBuilder ConfigureWorldSpawner(
        this ISpawnerConfigurationCollection configCollection,
        uint id)
        => configCollection
            .GetOrAddSpawnerConfiguration(new WorldSpawnerConfiguration())
            .GetBuilder(id);
}
