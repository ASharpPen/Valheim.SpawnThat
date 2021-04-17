using System.Runtime.CompilerServices;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    internal class CreatureSpawnerCache
    {
        public static ConditionalWeakTable<CreatureSpawner, CreatureSpawnerCache> Cache = new ConditionalWeakTable<CreatureSpawner, CreatureSpawnerCache>();

        public static CreatureSpawnerCache Get(CreatureSpawner spawner)
        {
            if (Cache.TryGetValue(spawner, out CreatureSpawnerCache cache))
            {
                return cache;
            }

            return null;
        }

        public static CreatureSpawnerCache Set(CreatureSpawner spawner, CreatureSpawnerConfig spawnConfig)
        {
            var cache = Cache.GetOrCreateValue(spawner);
            cache.Config = spawnConfig;

            return cache;
        }

        public CreatureSpawnerConfig Config;
    }
}
