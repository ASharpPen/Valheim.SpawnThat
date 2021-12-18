using UnityEngine;
using Valheim.SpawnThat.Configuration.ConfigTypes;
using Valheim.SpawnThat.Core;

namespace Valheim.SpawnThat.Spawners.SpawnerSpawnSystem.Conditions
{
    internal class ConditionDistanceToCenter : IConditionOnSpawn
    {
        private static ConditionDistanceToCenter _instance;

        public static ConditionDistanceToCenter Instance => _instance ??= new();

        public bool ShouldFilter(SpawnConditionContext context)
        {
            if (context is null || context.Config is null)
            {
                return false;
            }

            if (IsValid(context.Position, context.Config))
            {
                return false;
            }

            Log.LogTrace($"Filtering {context.Config.Name} due to distance to center.");
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
