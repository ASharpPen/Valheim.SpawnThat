using SpawnThat.Spawners.DestructibleSpawner;
using SpawnThat.Spawners.DestructibleSpawner.Configuration;

namespace SpawnThat.Spawners;

public static class ISpawnerConfigurationCollectionDestructibleSpawnerExtensions
{
    /// <summary>
    /// Create a new destructible spawner builder to configure.
    /// </summary>
    public static IDestructibleSpawnerBuilder ConfigureDestructibleSpawner(this ISpawnerConfigurationCollection configCollection)
        => configCollection
        .GetOrAddSpawnerConfiguration(new DestructibleSpawnerConfiguration())
        .CreateBuilder();
}
