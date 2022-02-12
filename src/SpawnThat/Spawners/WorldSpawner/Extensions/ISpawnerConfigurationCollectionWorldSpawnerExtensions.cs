using SpawnThat.Spawners.WorldSpawner;
using SpawnThat.Spawners.WorldSpawner.Configurations;

namespace SpawnThat.Spawners;

public static class ISpawnerConfigurationCollectionWorldSpawnerExtensions
{
    /// <summary>
    /// <para>Gets an IWorldSpawnBuilder with the indicated id. If none exist, creates a new one.</para>
    /// </summary>
    public static IWorldSpawnBuilder ConfigureWorldSpawner(
        this ISpawnerConfigurationCollection configCollection,
        uint id)
        => configCollection
            .GetOrAddSpawnerConfiguration(new WorldSpawnerConfiguration())
            .GetBuilder(id);
}
