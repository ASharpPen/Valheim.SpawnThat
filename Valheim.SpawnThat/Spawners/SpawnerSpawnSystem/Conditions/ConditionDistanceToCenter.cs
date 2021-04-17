
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
            var distance = spawner.transform.position.magnitude;

            if (distance < spawnConfig.ConditionDistanceToCenterMin.Value)
            {
                Log.LogTrace($"Ignoring world config {spawnConfig.Name} due to distance less than min.");
                return true;
            }

            if (spawnConfig.ConditionDistanceToCenterMax.Value > 0 && distance > spawnConfig.ConditionDistanceToCenterMax.Value)
            {
                Log.LogTrace($"Ignoring world config {spawnConfig.Name} due to distance greater than max.");
                return true;
            }

            return false;
        }
    }
}
