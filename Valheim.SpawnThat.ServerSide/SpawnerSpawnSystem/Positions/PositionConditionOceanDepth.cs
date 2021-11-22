using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class PositionConditionOceanDepth : ISpawnPositionCondition
    {
        public double? MinDepth { get; }
        public double? MaxDepth { get; }

        public PositionConditionOceanDepth(double? minDepth, double? maxDepth)
        {
            MinDepth = minDepth;
            MaxDepth = maxDepth;
        }

        public bool IsValid(PositionContext context)
        {
            var zone = WorldData.GetZone(context.Point);

            var oceanDepth = zone.OceanDepth(context.Point);

            if (MinDepth != null && oceanDepth < MinDepth)
            {
                return false;
            }

            if (MaxDepth != null && oceanDepth > MaxDepth)
            {
                return false;
            }

            return true;
        }
    }
}
