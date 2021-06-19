using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Valheim.SpawnThat.Spawners.Caches
{
    public static class SpawnSystemCache
    {
        private static ConditionalWeakTable<SpawnSystem, SpawnCounter> SpawnCounterTable = new();

        public static void SetCounter(SpawnSystem spawnSystem, SpawnCounter counter)
        {
            SpawnCounterTable.Add(spawnSystem, counter);
        }

        public static SpawnCounter GetCounter(SpawnSystem spawnSystem)
        {
            if (SpawnCounterTable.TryGetValue(spawnSystem, out SpawnCounter cached))
            {
                return cached;
            }

            return null;
        }
    }
}
