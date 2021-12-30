using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core.Cache;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner;

internal class CreatureSpawnerConfigCache
{
    private static ManagedCache<CreatureSpawnerConfigCache> Cache = new();

    public static CreatureSpawnerConfigCache Get(CreatureSpawner spawner)
    {
        if (Cache.TryGet(spawner, out CreatureSpawnerConfigCache cache))
        {
            return cache;
        }

        return null;
    }

    public static CreatureSpawnerConfigCache Set(CreatureSpawner spawner, CreatureSpawnerConfig spawnConfig)
    {
        var cache = Cache.GetOrCreate(spawner);
        cache.Config = spawnConfig;

        return cache;
    }

    public CreatureSpawnerConfig Config;
}
