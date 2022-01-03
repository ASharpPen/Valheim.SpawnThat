using Valheim.SpawnThat.Spawners.WorldSpawner;
using Valheim.SpawnThat.Spawners.WorldSpawner.Configurations;

namespace Valheim.SpawnThat.Spawners;

public static class SpawnerManagerWorldSpawnerExtensions
{
    public static IWorldSpawnBuilder ConfigureWorldSpawner(
        this ISpawnerConfigurationCollection configCollection,
        uint id)
        => configCollection
            .GetOrAddSpawnerConfiguration(new WorldSpawnConfigurationCollection())
            .GetBuilder(id);
}
