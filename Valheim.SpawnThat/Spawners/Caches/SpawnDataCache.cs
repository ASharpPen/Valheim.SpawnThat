using System.Runtime.CompilerServices;
using Valheim.SpawnThat.Configuration.ConfigTypes;

namespace Valheim.SpawnThat.Spawners.Caches
{
    internal class SpawnDataCache
    {
        public static ConditionalWeakTable<SpawnSystem.SpawnData, SpawnDataCache> SpawnSystemTable = new ConditionalWeakTable<SpawnSystem.SpawnData, SpawnDataCache>();

        public static SpawnDataCache Get(SpawnSystem.SpawnData spawner)
        {
            if (SpawnSystemTable.TryGetValue(spawner, out SpawnDataCache cache))
            {
                return cache;
            }

            return null;
        }

        public static SpawnDataCache Set(SpawnSystem.SpawnData spawner, SpawnConfiguration spawnConfig)
        {
            var cache = SpawnSystemTable.GetOrCreateValue(spawner);
            cache.Config = spawnConfig;

            return cache;
        }

        public SpawnConfiguration Config;
    }
}
