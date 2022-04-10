using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Zone;
using UnityEngine;

namespace SpawnThat.Options.PositionConditions;

/// <summary>
/// Required altitude range, based on terrain at spawner position.
/// </summary>
public class PositionConditionAltitude : ISpawnPositionCondition
{
    public float? Min { get; set; }

    public float? Max { get; set; }

    public PositionConditionAltitude()
    { }

    public PositionConditionAltitude(float? min, float? max)
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

        var altitude = ZoneManager
            .GetZone(position.GetZoneId())
            .Height(position)
            - ZoneSystem.instance.m_waterLevel;

        if (Min is not null &&
            Min < altitude)
        {
            return false;
        }

        if (Max is not null &&
            Max > altitude)
        {
            return false;
        }

        return true;
    }
}
