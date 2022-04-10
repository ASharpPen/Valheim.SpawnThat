using System;
using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Conditions;

public class ConditionTilt : ISpawnCondition
{
    public int? Min { get; set; }

    public int? Max { get;  set; }

    public ConditionTilt()
    { }

    public ConditionTilt(int? min, int? max)
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
