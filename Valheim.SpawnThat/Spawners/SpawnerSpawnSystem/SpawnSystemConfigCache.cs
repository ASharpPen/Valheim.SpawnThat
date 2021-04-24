using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Reset;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem
{
    internal class SpawnSystemConfigCache
    {
        public static ConditionalWeakTable<SpawnSystem.SpawnData, SpawnSystemConfigCache> SpawnSystemTable = new ConditionalWeakTable<SpawnSystem.SpawnData, SpawnSystemConfigCache>();

        public static SpawnSystemConfigCache Get(SpawnSystem.SpawnData spawner)
        {
            if (SpawnSystemTable.TryGetValue(spawner, out SpawnSystemConfigCache cache))
            {
                return cache;
            }

            return null;
        }

        public static SpawnSystemConfigCache Set(SpawnSystem.SpawnData spawner, SpawnConfiguration spawnConfig)
        {
            var cache = SpawnSystemTable.GetOrCreateValue(spawner);
            cache.Config = spawnConfig;

            return cache;
        }

        public SpawnConfiguration Config;
    }
}
