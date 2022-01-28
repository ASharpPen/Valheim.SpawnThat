using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SpawnThat.Options.Modifiers;

/// <summary>
/// Attempt to trigger vanilla run away and despawn, when no longer
/// fulfilling conditions for ConditionAllowDuringDay, ConditionAllowDuringNight and 
/// ConditionAllowDuringEnvironments.
/// </summary>
/// <remarks>Requires MonsterAI component on entity.</remarks>
public class ModifierDespawnOnConditionsInvalid : ISpawnModifier
{
    public const string ZdoConditionDay = "spawnthat_condition_daytime_day";
    public const string ZdoConditionNight = "spawnthat_condition_daytime_night";
    public const string ZdoConditionEnvironment = "spawnthat_condition_environments";
    public const string ZdoFeature = "spawnthat_despawn_on_invalid";

    public static int ZdoConditionDayHash { get; } = ZdoConditionDay.GetStableHashCode();
    public static int ZdoConditionNightHash { get; } = ZdoConditionNight.GetStableHashCode();
    public static int ZdoConditionEnvironmentHash { get; } = ZdoConditionEnvironment.GetStableHashCode();
    public static int ZdoFeatureHash { get; } = ZdoFeature.GetStableHashCode();

    public bool? ConditionAllowDuringDay { get; set; }
    public bool? ConditionAllowDuringNight { get; set; }
    public string ConditionAllowDuringEnvironments { get; set; }

    internal ModifierDespawnOnConditionsInvalid()
    { }

    public ModifierDespawnOnConditionsInvalid(
        bool? conditionAllowDuringDay = null,
        bool? conditionAllowDuringNight = null,
        List<string> conditionAllowDuringEnvironments = null)
    {
        ConditionAllowDuringDay = conditionAllowDuringDay;
        ConditionAllowDuringNight = conditionAllowDuringNight;
        ConditionAllowDuringEnvironments = conditionAllowDuringEnvironments?.Join();
    }

    public void Modify(GameObject entity, ZDO entityZdo)
    {
        if (entityZdo is null)
        {
            return;
        }

        entityZdo.Set(ZdoFeature, true);

        if (ConditionAllowDuringDay is not null)
        {
            entityZdo.Set(ZdoConditionDayHash, ConditionAllowDuringDay.Value);
        }
        if (ConditionAllowDuringNight is not null)
        {
            entityZdo.Set(ZdoConditionNightHash, ConditionAllowDuringNight.Value);
        }
        if (ConditionAllowDuringEnvironments is not null)
        {
            entityZdo.Set(ZdoConditionEnvironmentHash, ConditionAllowDuringEnvironments);
        }
    }
}
