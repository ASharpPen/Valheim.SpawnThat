using System.Collections.Generic;
using System.Linq;
using SpawnThat.Spawners.Contexts;
using SpawnThat.Utilities.Enums;

namespace SpawnThat.Options.Conditions;

public class ConditionDaytime : ISpawnCondition
{
    public Daytime[] Required { get; set; }

    public ConditionDaytime()
    { }

    public ConditionDaytime(bool allowDuringDay, bool allowDuringNight)
    {
        Required = (allowDuringDay, allowDuringNight) switch
        {
            (true, true) => new[] { Daytime.All },
            (true, false) => new[] { Daytime.Day },
            (false, true) => new[] { Daytime.Night },
            (false, false) => new Daytime[0],
        };
    }

    public ConditionDaytime(params Daytime[] daytimes)
    {
        Required = daytimes
            .OrderBy(x => x)
            .Distinct()
            .ToArray();
    }

    public ConditionDaytime(IEnumerable<Daytime> daytimes)
    {
        Required = daytimes
            .OrderBy(x => x)
            .Distinct()
            .ToArray();
    }

    public bool IsValid(SpawnSessionContext sessionContext)
    {
        if ((Required?.Length ?? 0) == 0)
        {
            return true;
        }

        var fraction = EnvMan.instance.GetDayFraction();

        return Required.Any(x => x.IsActive(fraction));
    }
}
