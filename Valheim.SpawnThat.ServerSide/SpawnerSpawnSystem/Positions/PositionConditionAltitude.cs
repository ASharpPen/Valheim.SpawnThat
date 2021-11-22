
namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    public class PositionConditionAltitude : ISpawnPositionCondition
    {
        public double? MinAltitude { get; }
        public double? MaxAltitude { get; }

        public PositionConditionAltitude(double? minAltitude, double? maxAltitude)
        {
            MinAltitude = minAltitude;
            MaxAltitude = maxAltitude;
        }

        public bool IsValid(PositionContext context)
        {
            float waterLevel = ZoneSystem.instance.m_waterLevel;

            float altitude = context.Point.y - waterLevel;

            if (MinAltitude != null && altitude < MinAltitude)
            {
                return false;
            }

            if (MaxAltitude != null && altitude > MaxAltitude)
            {
                return false;
            }

            return true;
        }
    }
}
