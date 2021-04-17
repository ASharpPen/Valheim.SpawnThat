using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    internal class SpawnSystemCache
    {
        public static ConditionalWeakTable<SpawnSystem.SpawnData, SpawnSystemCache> SpawnSystemTable = new ConditionalWeakTable<SpawnSystem.SpawnData, SpawnSystemCache>();

        public static SpawnSystemCache Get(SpawnSystem.SpawnData spawner)
        {
            if (SpawnSystemTable.TryGetValue(spawner, out SpawnSystemCache cache))
            {
                return cache;
            }

            return null;
        }

        public static SpawnSystemCache Set(SpawnSystem.SpawnData spawner, SpawnConfiguration spawnConfig)
        {
            var cache = SpawnSystemTable.GetOrCreateValue(spawner);
            cache.Config = spawnConfig;

            return cache;
        }

        public SpawnConfiguration Config;
    }
}
