using System.Runtime.CompilerServices;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerCreatureSpawner
{
    internal class CreatureSpawnerConfigCache
    {
        public static ConditionalWeakTable<CreatureSpawner, CreatureSpawnerConfigCache> Cache = new ConditionalWeakTable<CreatureSpawner, CreatureSpawnerConfigCache>();

        public static CreatureSpawnerConfigCache Get(CreatureSpawner spawner)
        {
            if (Cache.TryGetValue(spawner, out CreatureSpawnerConfigCache cache))
            {
                return cache;
            }

            return null;
        }

        public static CreatureSpawnerConfigCache Set(CreatureSpawner spawner, CreatureSpawnerConfig spawnConfig)
        {
            var cache = Cache.GetOrCreateValue(spawner);
            cache.Config = spawnConfig;

            return cache;
        }

        public CreatureSpawnerConfig Config;
    }
}
