using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Valheim.SpawnThat.ConfigurationCore;
using Valheim.SpawnThat.ConfigurationTypes;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
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
