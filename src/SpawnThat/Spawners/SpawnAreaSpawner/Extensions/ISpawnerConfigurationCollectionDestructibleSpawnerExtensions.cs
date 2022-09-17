using SpawnThat.Spawners.SpawnAreaSpawner;
using SpawnThat.Spawners.SpawnAreaSpawner.Configuration;

namespace SpawnThat.Spawners;

public static class ISpawnerConfigurationCollectionSpawnAreaSpawnerExtensions
{
    /// <summary>
    /// Create a new SpawnArea spawner builder to configure.
    /// </summary>
    public static ISpawnAreaSpawnerBuilder ConfigureSpawnAreaSpawner(this ISpawnerConfigurationCollection configCollection)
        => configCollection
        .GetOrAddSpawnerConfiguration(new SpawnAreaSpawnerConfiguration())
        .CreateBuilder();
}
