
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Models;
using Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.SpawnTemplates;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Conditions
{
    public class ConditionDistanceToCenter : ISpawnCondition
    {
        private double? MinDistance { get; }

        public double? MaxDistance { get; }

        public ConditionDistanceToCenter(double? minDistanceRequired, double? maxDistanceRequired)
        {
            MinDistance = minDistanceRequired > 0
                ? minDistanceRequired
                : null;
            MaxDistance = maxDistanceRequired > 0
                ? maxDistanceRequired
                : null;
        }

        public bool IsValid(SpawnSessionContext context, SpawnTemplate template)
        {
            if (MinDistance is null && MaxDistance is null)
            {
                return true;
            }

            var distance = context.SpawnSystemZDO.GetPosition().magnitude;

            if (MinDistance is not null 
                && distance < MinDistance)
            {
                return false;
            }

            if (MaxDistance is not null 
                && distance > MaxDistance)
            {
                return false;
            }

            return true;
        }
    }
}
