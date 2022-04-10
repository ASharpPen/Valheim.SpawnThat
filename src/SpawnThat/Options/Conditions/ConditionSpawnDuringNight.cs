using System;
using SpawnThat.Spawners.Contexts;

namespace SpawnThat.Options.Conditions;

[Obsolete("Use ConditionDaytime")]
public class ConditionSpawnDuringNight : ISpawnCondition
{
    public bool AllowedDuringNight { get; set; }

    public ConditionSpawnDuringNight()
    { }

    public ConditionSpawnDuringNight(bool allowDuringNight)
    {
        AllowedDuringNight = allowDuringNight;
    }

    public bool IsValid(SpawnSessionContext context)
    {
        if (!AllowedDuringNight && EnvMan.instance.IsNight())
        {
            return false;
        }

        return true;
    }
}
