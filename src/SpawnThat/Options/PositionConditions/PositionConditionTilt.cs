using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Zone;
using UnityEngine;

namespace SpawnThat.Options.PositionConditions;

public class PositionConditionTilt : ISpawnPositionCondition
{
    public float? Min { get; set; }

    public float? Max { get; set; }

    public PositionConditionTilt()
    { }

    public PositionConditionTilt(float? min, float? max)
    {
        Min = min;
        Max = max;
    }

    public bool IsValid(SpawnSessionContext sessionContext, Vector3 position)
    {
        if (Min is null &&
            Max is null)
        {
            return true;
        }

        var tilt = ZoneManager
            .GetZone(position.GetZoneId())
            .Tilt(position);

        if (Min is not null &&
            Min > tilt)
        {
            return false;
        }

        if (Max is not null &&
            Max < tilt)
        {
            return false;
        }

        return true;
    }
}
