// <summary>
// Unnecessary as long as the instance count itself is patched.
// </summary>

#if FALSE

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;
using Valheim.SpawnThat.Spawners.Caches;
using Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Managers;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{

    public class ConditionMaxSpawnedNonAI : IConditionOnSpawn
    {
        private static ConditionMaxSpawnedNonAI _instance;

        public static ConditionMaxSpawnedNonAI Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            var counter = SpawnSessionManager.Instance.GetService<SpawnCounter>();

            if (counter is null)
            {
                // Something is wrong. Skip this condition.
                return false;
            }

            if (IsValid(context.SpawnData.m_prefab, context.Config, counter))
            {
                return false;
            }

            Log.LogTrace($"Ignoring world config {context.Config.Name} due to too many entities spawned in area.");
            return true;
        }

        public bool IsValid(GameObject prefab, SpawnConfiguration config, SpawnCounter counter)
        {
            if(config is null || config.MaxSpawned.Value < 0)
            {
                return true;
            }

            if (config.MaxSpawned.Value == 0)
            {
                return false;
            }

            // Ignore if prefab is a normal creature / AI.
            var baseAI = PrefabCache.GetBaseAI(prefab);
            if (baseAI || baseAI is not null)
            {
                return true;
            }

            int instances = counter.CountInstancesInRange(prefab);

            if (instances < config.MaxSpawned.Value)
            {
                return true;
            }

            return false;
        }
    }
}
#endif
