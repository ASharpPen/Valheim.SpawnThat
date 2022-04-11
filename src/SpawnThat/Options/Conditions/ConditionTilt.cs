using System;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Conditions;

public class ConditionTilt : ISpawnCondition
{
    public float? Min { get; set; }

    public float? Max { get;  set; }

    public ConditionTilt()
    { }

    public ConditionTilt(float? min, float? max)
    {
        Min = min;
        Max = max;
    }

    public bool IsValid(SpawnSessionContext sessionContext)
    {
        if (Min is null &&
            Max is null)
        {
            return true;
        }

        var tilt = ZoneManager
            .GetZone(sessionContext.SpawnerZdo.GetSector())
            .Tilt(sessionContext.SpawnerZdo.GetPosition());

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
