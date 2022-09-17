using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Extensions;
using SpawnThat.World.Zone;
using UnityEngine;

namespace SpawnThat.Options.PositionConditions;

/// <summary>
/// <para>
///     Required distance to water surface.
/// </para>
/// <para>
///     Ocean depth is calculated as <c>Max(0, WaterLevel - Seafloor)</c>.
/// </para>
/// <para>
///     This means the water-surface and above will be ocean depth 0. 
///     An ocean floor at 10 meter below the surface will be 10.
/// </para>
/// </summary>
public class PositionConditionOceanDepth : ISpawnPositionCondition
{
    public float? Min { get; set; }

    public float? Max { get; set; }

    public PositionConditionOceanDepth()
    { }

    public PositionConditionOceanDepth(float? min, float? max)
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

        var depth = ZoneManager
            .GetZone(position.GetZoneId())
            .OceanDepth(position);

        if (Min is not null &&
            Min > depth)
        {
            return false;
        }

        if (Max is not null &&
            Max < depth)
        {
            return false;
        }

        return true;
    }
}
