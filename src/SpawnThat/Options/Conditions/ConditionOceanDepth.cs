using SpawnThat.Spawners.Contexts;
using SpawnThat.World.Zone;

namespace SpawnThat.Options.Conditions;

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
public class ConditionOceanDepth : ISpawnCondition
{
    public float? Min { get; set; }

    public float? Max { get; set; }

    public ConditionOceanDepth()
    { }

    public ConditionOceanDepth(float? min, float? max)
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

        var depth = ZoneManager
            .GetZone(sessionContext.SpawnerZdo.GetSector())
            .OceanDepth(sessionContext.SpawnerZdo.GetPosition());

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
