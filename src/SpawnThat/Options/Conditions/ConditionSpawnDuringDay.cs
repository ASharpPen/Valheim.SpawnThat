using System;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

[Obsolete("Use ConditionDaytime")]
public class ConditionSpawnDuringDay : ISpawnCondition
{
    public const string ZdoConditionDay = "spawnthat_condition_daytime_day";

    public bool AllowedDuringDay { get; set; }

    public ConditionSpawnDuringDay()
    { }

    public ConditionSpawnDuringDay(bool allowedDuringDay)
    {
        AllowedDuringDay = allowedDuringDay;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (!AllowedDuringDay && EnvMan.instance.IsDay())
        {
            return false;
        }

        return true;
    }
}
