using UnityEngine;
using Valheim.SpawnThat.Utilities.World;

namespace Valheim.SpawnThat.ServerSide.SpawnerSpawnSystem.Positions
{
    /// <summary>
    /// Basic estimation of terrain angle, based on looking at nearby world height
    /// and taking the average tilt from center point.
    /// </summary>
    /// <remarks>Does not currently take into account terrain changes!</remarks>
    public class PositionConditionSurfaceTilt : ISpawnPositionCondition
    {
        private float MinTilt { get; }
        private float MaxTilt { get; }

        public PositionConditionSurfaceTilt(float minTilt, float maxTilt)
        {
            MinTilt = minTilt;
            MaxTilt = maxTilt;
        }

        public bool IsValid(PositionContext context)
        {
            var avgAngle = WorldData.Tilt(context.Point);

            if (avgAngle < MinTilt)
            {
#if false && DEBUG
                Log.LogTrace("Tilt: " + avgAngle + ", Min: " + MinTilt);
#endif
                return false;
            }

            if (avgAngle > MaxTilt)
            {
#if false && DEBUG
                Log.LogTrace("Tilt: " + avgAngle + ", Max " + MaxTilt);
#endif
                return false;
            }

            return true;
        }
    }
}
