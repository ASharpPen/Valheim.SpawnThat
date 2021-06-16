
using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    internal class ConditionDistanceToCenter : IConditionOnAwake
    {
        private static ConditionDistanceToCenter _instance;

        public static ConditionDistanceToCenter Instance
        {
            get
            {
                return _instance ??= new ConditionDistanceToCenter();
            }
        }

        public bool ShouldFilter(SpawnSystem spawner, SpawnConfiguration spawnConfig)
        {
            if(!spawner || spawner is null || spawnConfig is null)
            {
                return false;
            }

            if(IsValid(spawner.transform.position, spawnConfig))
            {
                return false;
            }

            Log.LogTrace($"Filtering {spawnConfig.Name} due to distance to center.");
            return true;
        }

        public bool IsValid(Vector3 position, SpawnConfiguration config)
        {
            var distance = position.magnitude;

            if (distance < config.ConditionDistanceToCenterMin.Value)
            {
                return false;
            }

            if (config.ConditionDistanceToCenterMax.Value > 0 && distance > config.ConditionDistanceToCenterMax.Value)
            {
                return false;
            }

            return true;
        }
    }
}
