using SpawnThat.Options.PositionConditions;
using SpawnThat.Utilities.Enums;
using System.Collections.Generic;

namespace SpawnThat.Spawners;

public static class IHaveSpawnPositionConditionsExtensions
{
    /// <summary>
    /// <para> Set allowed altitude range in which spawning is active. </para>
    /// <para> 
    ///     Altitude is calculated as the vertical distance to water-level.
    ///     Meaning altitude 0 is at the water surface. 
    ///     If negative, it means below water, positive means above.
    /// </para>
    /// </summary>
    /// <param name="min">Sets minimum altitude for spawning to be active. Ignored if null.</param>
    /// <param name="max">Sets maximum altitude for spawning to be active. Ignored if null.</param>
    public static T SetPositionConditionAltitude<T>(this T builder, float? min = null, float? max = null)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionAltitude(min, max));
        return builder;
    }

    /// <summary>
    /// <para>Set forestation state for which spawning is active.</para>
    /// <para>Note: This is based on worldgeneration, not on actual amount of vegetation present.</para>
    /// </summary>
    public static T SetPositionConditionForest<T>(this T builder, ForestState requiredState)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionForest(requiredState));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed during spawn position checks.
    /// </summary>
    public static T SetPositionConditionLocation<T>(this T builder, IEnumerable<string> locationNames)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }

    /// <summary>
    /// Sets locations in which spawning is allowed during spawn position checks.
    /// </summary>
    public static T SetPositionConditionLocation<T>(this T builder, params string[] locationNames)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionLocation(locationNames));
        return builder;
    }

    /// <summary>
    /// <para>Set allowed ocean depth range in which spawning is active.</para>
    /// <para>Ocean depth is defined as distance to water surface. And is calculated as:</para>
    /// <para><c>Max(0, WaterLevel - Seafloor)</c></para>
    /// <para>
    ///     This means the water-surface and above will be ocean depth 0. 
    ///     An ocean floor at 10 units below the surface will be 10.
    /// </para>
    /// </summary>
    /// <param name="min">Minimum depth required. Ignored if null.</param>
    /// <param name="max">Maximum depth allowed. Ignored if null.</param>
    public static T SetPositionConditionOceanDepth<T>(this T builder, float? min = null, float? max = null)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionOceanDepth(min, max));
        return builder;
    }

    /// <summary>
    /// <para>Set tilt range of terrain, for which spawning is active.</para>
    /// <para>Tilt is calculated as 0 when perfectly flat, and 90 when completely vertical.</para>
    /// <para>
    ///     Note: Valheim calculates tilt extremely inconsistently throughout the codebase.
    ///     Tilt above 45 needs to be tested before being relied upon, as there is at least one
    ///     buggy calculation in vanilla.
    /// </para>
    /// </summary>
    /// <param name="min">Minimum degrees of tilt required. Ignored if null.</param>
    /// <param name="max">Maximum degrees of tilt allowed. Ignored if null.</param>
    public static T SetPositionConditionTilt<T>(this T builder, float? min = null, float? max = null)
        where T : IHaveSpawnPositionConditions
    {
        builder.SetPositionCondition(new PositionConditionTilt(min, max));
        return builder;
    }
}
