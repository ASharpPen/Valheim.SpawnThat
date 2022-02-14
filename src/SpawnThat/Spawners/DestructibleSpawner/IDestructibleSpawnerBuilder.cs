using SpawnThat.Spawners.DestructibleSpawner.Identifiers;

namespace SpawnThat.Spawners.DestructibleSpawner;

/// <summary>
/// Configuration for a destructible spawner (eg. a greydwarf nest).
/// 
/// To identify which spawner to modify, set <c>ISpawnerIdentifier</c>'s 
/// on the builder.
/// 
/// Spawner controls the general settings for when things should spawn.
/// It selects what to spawn from a list of spawns, which can be modified
/// or added to by using <c>IDestructibleSpawnBuilder</c>.
/// </summary>
public interface IDestructibleSpawnerBuilder
{
    /// <summary>
    /// Get or create a spawn builder for spawner.
    /// 
    /// If id matches the index of an existing spawn, the existing spawn will be
    /// overridden by the assigned settings of the builder.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    IDestructibleSpawnBuilder GetSpawnBuilder(uint id);

    IDestructibleSpawnerBuilder SetIdentifier<T>(T identifier)
        where T : class, ISpawnerIdentifier;
}
