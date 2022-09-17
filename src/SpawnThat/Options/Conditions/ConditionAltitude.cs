using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Conditions;

/// <summary>
/// Required altitude range, based on terrain at spawner position.
/// </summary>
public class ConditionAltitude : ISpawnCondition
{
    public float? Min { get; set; }

    public float? Max { get; set; }

    public ConditionAltitude()
    { }

    public ConditionAltitude(float? min, float? max)
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

        var altitude = ZoneManager
            .GetZone(sessionContext.SpawnerZdo.GetSector())
            .Height(sessionContext.SpawnerZdo.GetPosition())
            - ZoneSystem.instance.m_waterLevel;

        if (Min is not null &&
            Min > altitude)
        {
            return false;
        }

        if (Max is not null &&
            Max < altitude)
        {
            return false;
        }

        return true;
    }
}
